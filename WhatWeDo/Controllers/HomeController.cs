using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using WhatWeDo.Models;
using WhatWeDo.Servicios.Contratos;

namespace WhatWeDo.Controllers
{
    public class HomeController : Controller
    {

        private readonly IEventoService _ServicioEvento;
   
        static List<Evento> lstEventosCategorizados = new List<Evento>();
        static bool bBuscarPorCategoria = false;

        private const int PageSize = 5; // Número de eventos por página

        public HomeController(IEventoService servicioEvento)
        {
            _ServicioEvento = servicioEvento;
        }

        public IActionResult Inicio()
        {
            return RedirectToAction("Eventos", "Home");
        }

        public async Task<IActionResult> Eventos(int page = 1)
        {
            List<Evento> lstEventos = new List<Evento>();

            // Para redirigir a los eventos por categoría
            if (bBuscarPorCategoria)
            {
                lstEventos = lstEventosCategorizados;
                bBuscarPorCategoria = false;
            }
            else
            {
                lstEventos = await _ServicioEvento.GetEventos(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            // Implementar la paginación
            int totalEventos = lstEventos.Count;
            int totalPages = (int)Math.Ceiling((double)totalEventos / PageSize);
            var paginatedEvents = lstEventos.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.PaginaActual = page;
            ViewBag.TotalPaginas = totalPages;

            return View(paginatedEvents);
        }

        public async Task<IActionResult> EventosPorCategoria(int categoria)
        {
            bBuscarPorCategoria = true;
            lstEventosCategorizados = await _ServicioEvento.GetEventosPorCategoria(categoria);

            return RedirectToAction("Eventos", "Home");
        }     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}