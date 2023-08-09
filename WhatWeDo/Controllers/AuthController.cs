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

            if (usuario.Email!=null && usuario.Pass != null)
            {
                oUsuario = await _Servicio.GetUsuario(usuario.Email, Utilidades.EncriptarPassword(usuario.Pass));
            }
            

            if (oUsuario.IdUsuario == 0)
            {
                ViewBag.Alert = "Email y/o contraseña inválidos.";
                return View("Login");
            }
            string rol = "Usuario";
            
            if (oUsuario.Rol)
                rol = "Empresa";

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, oUsuario.NombreUsuario),
                new Claim("Email", oUsuario.Email),
                new Claim(ClaimTypes.Role, rol)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CrearUsuario(Usuario usuario)
        {            
            usuario.Pass = Utilidades.EncriptarPassword(usuario.Pass);

            string sTransaccion = await _Servicio.CrearUsuario(usuario);
            if (sTransaccion.Equals("NOK"))
            {
                ViewBag.Alert = "El email proporcionado ya esta en uso.";
                return View("Register");
            }
            return Redirect("Login");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Todos", "Home");
        }

    }
}
