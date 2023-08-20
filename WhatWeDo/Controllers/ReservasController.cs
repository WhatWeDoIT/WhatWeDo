using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatWeDo.Models;
using WhatWeDo.Servicios.Contratos;

namespace WhatWeDo.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IEventoService _ServicioEvento;

        static List<Evento> lstEventosCategorizados = new List<Evento>();
        static bool bBuscarPorCategoria = false;

        public ReservasController(IEventoService servicioEvento)
        {
            _ServicioEvento = servicioEvento;
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> MisReservas()
        {
            List<Evento> lstEventos = new List<Evento>();

            //Para redirigir a los eventos por categoria
            if (bBuscarPorCategoria)
            {
                lstEventos = lstEventosCategorizados;
                bBuscarPorCategoria = false;
            }
            else
            {
                lstEventos = await _ServicioEvento.GetEventosPorUsuario(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            return View(lstEventos);
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> EventosPorCategoria(int categoria)
        {
            bBuscarPorCategoria = true;
            lstEventosCategorizados = await _ServicioEvento.GetEventosPorUsuarioCategoria(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), categoria);

            return RedirectToAction("MisReservas", "Reservas");
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> AnularReserva(int idEvento)
        {
            await _ServicioEvento.AnularReservaEvento(idEvento, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return RedirectToAction("MisReservas", "Reservas");
        }
    }
}
