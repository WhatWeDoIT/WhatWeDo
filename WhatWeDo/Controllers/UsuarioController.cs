using Microsoft.AspNetCore.Mvc;
using WhatWeDo.Models;

namespace WhatWeDo.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Modify()
        {
            return View();
        }
        //public async Task<IActionResult> ModificarUsuario()
        //{
        //    string sTransaccion = await _Servicio.CrearUsuario(usuario);
        //    if (sTransaccion.Equals("NOK"))
        //    {
        //        ViewBag.Alert = "El email proporcionado ya esta en uso.";
        //        return View("Register");
        //    }
        //    return Redirect("Login");
        //}
    }
}
