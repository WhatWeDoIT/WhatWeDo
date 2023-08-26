using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WhatWeDo.Models;
using WhatWeDo.Recursos;
using WhatWeDo.Servicios.Contratos;

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
        public async Task<IActionResult> ReservarEvento(EventoPago eventoPago, int idEvento)
        {
            Evento oEvento = await _ServicioEvento.GetEventoPorId(Convert.ToInt32(idEvento));
            Empresa oEmpresa = await _ServicioEmpresa.GetEmpresaPorIdEvento(Convert.ToInt32(idEvento));
            Usuario oUsuario = await _ServicioUsuario.GetUsuario(User.FindFirstValue(ClaimTypes.Email));
            
            eventoPago.FkIdEvento = oEvento.IdEvento;
            eventoPago.FkIdUsuario = oUsuario.IdUsuario;
            eventoPago.FkIdEmpresa = oEmpresa.IdEmpresa;
            
            eventoPago.Evento = oEvento;
            eventoPago.PrecioConSimbolo = eventoPago.Evento.Precio.ToString() + "€";

            //Carga de combos
            eventoPago.FechasEvento = await _ServicioEvento.GetFechasEvento(oEvento.FechaInicio, oEvento.FechaFin);
            ViewBag.fechasEvento = new SelectList(eventoPago.FechasEvento, "IdListaFecha", "ItemFechaFormateada");

            eventoPago.MiembrosEvento = _ServicioEvento.GetMiembrosEvento(eventoPago);
            ViewBag.miembros = new SelectList(eventoPago.MiembrosEvento, "IdListaMiembro", "ItemMiembro");
            return View(eventoPago);
            
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> PagoEvento(EventoPago eventoPago)
        {
            await _ServicioEvento.InsertEventoPago(eventoPago);

            await _ServicioEvento.ReservarEvento(eventoPago.FkIdEvento, eventoPago.FkIdUsuario
                                               , eventoPago.FechaAsistencia, eventoPago.Miembros);

            await _ServicioUsuario.PagarEvento(eventoPago.FkIdUsuario, eventoPago.FkIdEmpresa
                                             , eventoPago.PrecioTotal, eventoPago.PuntosAsignados);

            Usuario oUsuario = await _ServicioUsuario.GetUsuario(User.FindFirstValue(ClaimTypes.Email));

            return RedirectToAction("ActualizarSaldo", "Auth", oUsuario);
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> MisReservas(int page = 1)
        {
            List<Evento> lstEventos = new List<Evento>();

            //Para redirigir a los eventos por categoria
            if (bBuscarPorCategoria)
            {
                lstEventos = lstEventosCategorizados;
                string categoriaSeleccionada = TempData["CategoriaSeleccionada"] as string;

                if (categoriaSeleccionada != null)
                {
                    ViewData["CetegoriaRecibida"] = categoriaSeleccionada;
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

            return RedirectToAction("ActualizarSaldo", "Reservas");
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
