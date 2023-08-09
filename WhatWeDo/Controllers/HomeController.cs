using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WhatWeDo.Models;

namespace WhatWeDo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Todos()
        {
            
            return View();
        }

        [Authorize(Roles = "Usuario, Empresa")]
        public IActionResult Index()
        {
            Usuario usuario = new Usuario();
            usuario.NombreUsuario = User.FindFirstValue(ClaimTypes.Name);            
            return View(usuario);
        }

        [Authorize(Roles = "Empresa")]
        public IActionResult Privacy()
        {
            return View();
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}