using Microsoft.AspNetCore.Mvc;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Models;
using WhatWeDo.Recursos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace WhatWeDo.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly IUsuarioService _Servicio;

        public AuthController(IUsuarioService servicio)
        {
            _Servicio = servicio;                
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> IniciarSesion(Usuario usuario)
        {
            Usuario oUsuario = new Usuario();

            if (usuario.Mail!=null && usuario.Pass != null)
            {
                oUsuario = await _Servicio.LoginUsuario(usuario.Mail, Utilidades.EncriptarPassword(usuario.Pass));
            }

            if (oUsuario.IdUsuario == 0)
            {
                ViewBag.Alert = "Email y/o contraseña inválidos.";
                return View("Login");
            }

            //Funcion para saber si es usuario o empresa aqui

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, oUsuario.Nombre),
                new Claim("Mail", oUsuario.Mail),
                //new Claim(ClaimTypes.Role, rol)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Eventos", "Home");
        }

        public async Task<IActionResult> CrearUsuario(Usuario usuario)
        {            
            usuario.Pass = Utilidades.EncriptarPassword(usuario.Pass);

            if (!usuario.EsEmpresa)
            {
                string sTransaccion = await _Servicio.InsertUsuario(usuario);
                if (sTransaccion.Equals("NOK"))
                {
                    ViewBag.Alert = "El email proporcionado ya esta en uso.";
                    return View("Register");
                }               
            }
            else
            {
                ViewBag.Alert = "Usuario empresa falta por implementar";
                return View("Register");
            }

            return Redirect("Login");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Eventos", "Home");
        }

    }
}
