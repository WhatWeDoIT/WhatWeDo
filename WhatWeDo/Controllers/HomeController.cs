using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
        private readonly IEmpresaService _ServicioEmpresa;

        static List<Evento> lstEventosCategorizados = new List<Evento>();
        static bool bBuscarPorCategoria = false;

        private const int PageSize = 3; // Número de eventos por página

        public HomeController(IEventoService servicioEvento, IEmpresaService servicioEmpresa)
        {
            _ServicioEvento = servicioEvento;
            _ServicioEmpresa = servicioEmpresa;
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
                string categoriaSeleccionada = TempData["CategoriaSeleccionada"] as string;

                if (categoriaSeleccionada != null)
                {
                    ViewData["CetegoriaRecibida"] = categoriaSeleccionada;
                }

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


            //actualizamos el saldo de la empresa
            Empresa oEmpresa = await _ServicioEmpresa.GetEmpresa(User.FindFirstValue(ClaimTypes.Email));

            await ActualizarSaldoEmpresa(oEmpresa);

            return View(paginatedEvents);
        }
        public async Task ActualizarSaldoEmpresa(Empresa empresa)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            string rol = "Empresa";

            Empresa oEmpresa = await _ServicioEmpresa.GetEmpresa(empresa.Mail);
            empresa.Nombre = oEmpresa.Nombre;
            empresa.IdEmpresa = oEmpresa.IdEmpresa;
            empresa.Saldo = oEmpresa.Saldo;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, empresa.Nombre),
                new Claim(ClaimTypes.Email, empresa.Mail),
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.NameIdentifier, empresa.IdEmpresa.ToString()),
                new Claim("saldo", empresa.Saldo.ToString() + " €")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
        public async Task<IActionResult> EventosPorCategoria(int categoria)
        {
            bBuscarPorCategoria = true;
            lstEventosCategorizados = await _ServicioEvento.GetEventosPorCategoria(categoria);
            //Enviamos la cetegoria al controller siguiente
            TempData["CategoriaSeleccionada"] = categoria.ToString();

            return RedirectToAction("Eventos", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}