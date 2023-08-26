using Microsoft.AspNetCore.Mvc;

namespace WhatWeDo.Controllers
{
    public class UbicacionController : Controller
    {
        public IActionResult Maps(string direccion, string titulo)
        {
            ViewBag.Direccion = direccion;
            ViewBag.Titulo = titulo;    

            return View();
        }
    }
}
