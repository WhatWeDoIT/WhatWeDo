using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Claims;
using WhatWeDo.Models;
using WhatWeDo.Recursos;
using WhatWeDo.Servicios.Contratos;
using static WhatWeDo.Recursos.CargasCombo;

namespace WhatWeDo.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IEventoService _ServicioEvento;
        private readonly IEmpresaService _ServicioEmpresa;
        private readonly IUsuarioService _ServicioUsuario;


        static List<Evento> lstEventosCategorizados = new List<Evento>();
        static bool bBuscarPorCategoria = false;

        private const int PageSize = 3; // Número de eventos por página

        public ReservasController(IEventoService servicioEvento
                                , IEmpresaService servicioEmpresa
                                , IUsuarioService servicioUsuario)
        {
            _ServicioEvento = servicioEvento;
            _ServicioEmpresa = servicioEmpresa;
            _ServicioUsuario = servicioUsuario;           
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> ReservarEvento(EventoPago eventoPago, int idEvento, string mensajeValidacion)
        {
            if (mensajeValidacion != null)
                ViewBag.Alert = mensajeValidacion;

            if (TempData.ContainsKey("EventoPagoTemp"))
            {
                string eventoPagoJson = TempData["EventoPagoTemp"] as string;
                eventoPago = JsonConvert.DeserializeObject<EventoPago>(eventoPagoJson);
            }

            Evento oEvento = await _ServicioEvento.GetEventoPorId(Convert.ToInt32(idEvento));
            Empresa oEmpresa = await _ServicioEmpresa.GetEmpresaPorIdEvento(Convert.ToInt32(idEvento));
            Usuario oUsuario = await _ServicioUsuario.GetUsuario(User.FindFirstValue(ClaimTypes.Email));
            
            eventoPago.FkIdEvento = oEvento.IdEvento;
            eventoPago.FkIdUsuario = oUsuario.IdUsuario;
            eventoPago.FkIdEmpresa = oEmpresa.IdEmpresa;
            
            eventoPago.Evento = oEvento;
            eventoPago.PrecioConSimbolo = eventoPago.Evento.Precio.ToString() + "€";

            //Carga de combos
            eventoPago.FechasEvento = await _ServicioEvento.GetFechasEvento(oEvento.FechaInicio.Value, oEvento.FechaFin.Value);
            if (eventoPago.FechaAsistencia != DateTime.MinValue)
            {
                FechasEvento fechaSeleccionada = eventoPago.FechasEvento.FirstOrDefault(f => f.ItemFechaFormateada == eventoPago.FechaAsistencia.ToString("dd/MM/yyyy"));
                ViewBag.fechasEvento = new SelectList(eventoPago.FechasEvento, "IdListaFecha", "ItemFechaFormateada", fechaSeleccionada.IdListaFecha);
            }
            else
            {
                ViewBag.fechasEvento = new SelectList(eventoPago.FechasEvento, "IdListaFecha", "ItemFechaFormateada");
            }

            eventoPago.MiembrosEvento = _ServicioEvento.GetMiembrosEvento(eventoPago);
            if (eventoPago.Miembros != 0)
            {
                MiembrosEvento miembroSelecciondado = eventoPago.MiembrosEvento.FirstOrDefault(f => f.ItemMiembro == eventoPago.Miembros);
                ViewBag.miembros = new SelectList(eventoPago.MiembrosEvento, "IdListaMiembro", "ItemMiembro", miembroSelecciondado.IdListaMiembro);
            }
            else
            {
                ViewBag.miembros = new SelectList(eventoPago.MiembrosEvento, "IdListaMiembro", "ItemMiembro");
            }
                
            return View(eventoPago);
            
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> PagoEvento(EventoPago eventoPago)
        {
            bool bValidacion = true;
            string sMensaje = null;

            if (eventoPago.FechaAsistencia == DateTime.MinValue)
            {
                sMensaje += "La fecha de asistencia es oligatoria\n";
                bValidacion = false;
            }

            if(eventoPago.Miembros == 0)
            {
                sMensaje += "El nº de reservas es obligatorio\n";
                bValidacion = false;
            }

            if (!bValidacion)
            {
                // Serializa el objeto EventoPago a formato JSON
                string eventoPagoJson = JsonConvert.SerializeObject(eventoPago);

                TempData["EventoPagoTemp"] = eventoPagoJson;
                return RedirectToAction("ReservarEvento", new { mensajeValidacion = sMensaje, idEvento = eventoPago.FkIdEvento });
            }

            await _ServicioEvento.InsertEventoPago(eventoPago);

            await _ServicioEvento.ReservarEvento(eventoPago.FkIdEvento, eventoPago.FkIdUsuario
                                               , eventoPago.FechaAsistencia, eventoPago.Miembros);

            await _ServicioUsuario.PagarEvento(eventoPago.FkIdUsuario, eventoPago.FkIdEmpresa
                                             , eventoPago.PrecioTotal, eventoPago.PuntosAsignados);

            Usuario oUsuario = await _ServicioUsuario.GetUsuario(User.FindFirstValue(ClaimTypes.Email));

            return RedirectToAction("ActualizarSaldo", "Auth", oUsuario);
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> MisReservas(int paginacionCategoria, int idCategoria, int page = 1)
        {
            List<Evento> lstEventos = new List<Evento>();
           
            bool bPaginacionCategoria = false;

            if (paginacionCategoria != 0)
                bPaginacionCategoria = true;

            //Para redirigir a los eventos por categoria
            if (bBuscarPorCategoria || bPaginacionCategoria)
            {
                lstEventos = lstEventosCategorizados;
                string categoriaSeleccionada = TempData["CategoriaSeleccionada"] as string;

                if (categoriaSeleccionada != null)
                {
                    ViewData["CategoriaRecibida"] = categoriaSeleccionada;
                }


                if (idCategoria != 0)
                {
                    ViewData["CategoriaRecibida"] = idCategoria;
                }

                bBuscarPorCategoria = false;
            }
            else
            {
                lstEventos = await _ServicioEvento.GetEventosPorUsuario(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            // Implementar la paginación
            int totalEventos = lstEventos.Count;
            int totalPages = (int)Math.Ceiling((double)totalEventos / PageSize);
            var paginatedEvents = lstEventos.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.PaginaActual = page;
            ViewBag.TotalPaginas = totalPages;

            return View(paginatedEvents);
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> EventosPorCategoria(int categoria)
        {
            bBuscarPorCategoria = true;
            lstEventosCategorizados = await _ServicioEvento.GetEventosPorUsuarioCategoria(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), categoria);
            //Enviamos la cetegoria al controller siguiente
            TempData["CategoriaSeleccionada"] = categoria.ToString();

            var routeValues = new RouteValueDictionary
            {
                { "paginacionCategoria", categoria },
                { "idCategoria", categoria }
            };



            return RedirectToAction("MisReservas", "Reservas", routeValues);
        }      
       
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> AnularReserva(int idEvento)
        {
            EventoPago oEventoPago = await _ServicioEvento.GetEventoPago(idEvento, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            await _ServicioUsuario.DevolucionEventoPago(oEventoPago.FkIdUsuario, oEventoPago.FkIdEmpresa
                                                      , oEventoPago.PrecioTotal, oEventoPago.PuntosAsignados);
            
            await _ServicioEvento.AnularReservaEvento(oEventoPago.FkIdEvento, oEventoPago.FkIdUsuario, oEventoPago.Miembros);

            Usuario oUsuario = await _ServicioUsuario.GetUsuario(User.FindFirstValue(ClaimTypes.Email));

            return RedirectToAction("ActualizarSaldo", "Auth", oUsuario);
        }
    }
}
