using Microsoft.AspNetCore.Mvc;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Models;
using WhatWeDo.Recursos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;

namespace WhatWeDo.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly IUsuarioService _ServicioUsuario;
        private readonly IEmpresaService _ServicioEmpresa;

        public AuthController(IUsuarioService servicioUsuario, IEmpresaService servicioEmpresa)
        {
            _ServicioUsuario = servicioUsuario;
            _ServicioEmpresa = servicioEmpresa;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Preferences()
        {
            // Supongamos que tienes una lista de categorías de eventos
            //List<CategoriaEvento> categorias = ObtenerCategoriasDesdeLaBaseDeDatos(); //TODO
            return View();
        }
        public async Task<IActionResult> IniciarSesion(Usuario usuario)
        {
            Usuario oUsuario = new Usuario();
            string rol;
            if (!usuario.EsEmpresa)
            {
                if (usuario.Mail != null && usuario.Pass != null)
                {
                    oUsuario = await _ServicioUsuario.LoginUsuario(usuario.Mail, Utilidades.EncriptarPassword(usuario.Pass));
                    usuario.Nombre = oUsuario.Nombre;
                }

                if (oUsuario.IdUsuario == 0)
                {
                    ViewBag.Alert = "Email y/o contraseña inválidos.";
                    return View("Login");
                }
                rol = "Usuario";
            }
            else
            {
                //Convertimos el usuario a empresa para hacer el insert a la tabla que toca
                Empresa oEmpresa = new Empresa(0, usuario.Nombre, usuario.Pass, usuario.Direccion, usuario.Mail);

                if (usuario.Mail != null && usuario.Pass != null)
                {
                    oEmpresa = await _ServicioEmpresa.LoginEmpresa(oEmpresa.Mail, Utilidades.EncriptarPassword(oEmpresa.Pass));
                    usuario.Nombre = oEmpresa.Nombre;
                }

                if (oEmpresa.IdEmpresa == 0)
                {
                    ViewBag.Alert = "Email y/o contraseña inválidos.";
                    return View("Login");
                }
                rol = "Empresa";
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Mail),
                new Claim(ClaimTypes.Role, rol)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Eventos", "Home");
        }

        public async Task<IActionResult> CrearUsuario(Usuario usuario)
        {
            //Comprobar que los campos no esten vacios
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || 
                string.IsNullOrWhiteSpace(usuario.Mail) || string.IsNullOrWhiteSpace(usuario.Pass) ||
                string.IsNullOrWhiteSpace(usuario.ConfirmPass))
            {
                ViewBag.Alert = "Por favor, complete todos los campos.";
                return View("Register");
            }

            //Comprobar que todos los datos tengan un formato valido
            if (!ValidarRequisitosNombre(usuario.Nombre) || 
                !ValidarRequisitosEmail(usuario.Mail) || !ValidarRequisitosPassword(usuario.Pass) || usuario.Pass != usuario.ConfirmPass)
            {
                ViewBag.Alert = "Alguno de los campos no cumple con los requisitos.";
                return View("Register");
            }

            //Añadir al usuario a la base de datos
            usuario.Pass = Utilidades.EncriptarPassword(usuario.Pass);//Encriptar contraseña
            if (!usuario.EsEmpresa)
            {
                string sTransaccion = await _ServicioUsuario.InsertUsuario(usuario);
                if (sTransaccion.Equals("NOK"))
                {
                    ViewBag.Alert = "El email proporcionado ya esta en uso.";
                    return View("Register");
                }
            }
            else
            {
                //Convertimos el usuario a empresa para hacer el insert a la tabla que toca
                Empresa oEmpresa = new Empresa(0, usuario.Nombre, usuario.Pass, usuario.Direccion, usuario.Mail);

                if (!ValidarRequisitosDireccion(usuario.Direccion))
                {
                    ViewBag.Alert = "Alguno de los campos no cumple con los requisitos.";
                    return View("Register");
                }
                
                string sTransaccion = await _ServicioEmpresa.InsertEmpresa(oEmpresa);
                if (sTransaccion.Equals("NOK"))
                {
                    ViewBag.Alert = "El email proporcionado ya esta en uso.";
                    return View("Register");
                }
            }

            return Redirect("Login");

        }
        private bool ValidarRequisitosNombre(string nombre)
        {
            // Verificar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return false;
            }

            // Eliminar espacios en blanco al principio y al final
            nombre = nombre.Trim();

            // Verificar la longitud del nombre
            if (nombre.Length < 3 || nombre.Length > 20)
            {
                return false;
            }

            // Verificar que solo contenga letras, números y espacios
            var validCharacters = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9\\s]*$");
            if (!validCharacters.IsMatch(nombre))
            {
                return false;
            }

            return true;
        }


        private bool ValidarRequisitosDireccion(string direccion)
        {
            // Verificar que la dirección no esté vacía
            if (string.IsNullOrWhiteSpace(direccion))
            {
                return false;
            }

            // Eliminar espacios en blanco al principio y al final
            direccion = direccion.Trim();

            // Verificar la longitud de la dirección
            if (direccion.Length < 10 || direccion.Length > 50)
            {
                return false;
            }

            return true;
        }


        private bool ValidarRequisitosEmail(string email)
        {
            // Verificar que el email no esté vacío
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            // Eliminar espacios en blanco al principio y al final
            email = email.Trim();

            // Verificar el formato del email utilizando una expresión regular
            string validEmailPattern = "^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$";
            Regex regex = new Regex(validEmailPattern);
            if (!regex.IsMatch(email))
            {
                return false;
            }

            return true;
        }

        private bool ValidarRequisitosPassword(string contraseña)
        {
            // Verificar que la contraseña no esté vacía
            if (string.IsNullOrWhiteSpace(contraseña))
            {
                return false;
            }

            // Eliminar espacios en blanco al principio y al final
            contraseña = contraseña.Trim();

            // Verificar que la contraseña cumple con los requisitos utilizando una expresión regular
            string validPasswordPattern = "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d!$%@#£€*?&]{8,}$";
            Regex regex = new Regex(validPasswordPattern);
            if (!regex.IsMatch(contraseña))
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Eventos", "Home");
        }

    }
}
