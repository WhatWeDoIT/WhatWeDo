﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(IEventoService servicioEvento)
        {
            _ServicioEvento = servicioEvento;          
        }

        public IActionResult Inicio()
        {
            return RedirectToAction("Eventos", "Home");
        }

        public async Task <IActionResult> Eventos()
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
                lstEventos = await _ServicioEvento.GetEventos(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }           

            return View(lstEventos);
        }

        public async Task<IActionResult> EventosPorCategoria(int categoria)
        {
            bBuscarPorCategoria = true;
            lstEventosCategorizados = await _ServicioEvento.GetEventosPorCategoria(categoria);

            return RedirectToAction("Eventos", "Home");
        }       
        
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> ReservarEvento(int idEvento)
        {
            await _ServicioEvento.ReservarEvento(idEvento, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return RedirectToAction("MisReservas", "Reservas");
        }
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}