using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using WhatWeDo.Models;
using WhatWeDo.Recursos;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Servicios.Implementacion;

namespace WhatWeDo.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _ServicioUsuario;
        private readonly IEmpresaService _ServicioEmpresa;

        public UsuarioController(IUsuarioService servicioUsuario, IEmpresaService servicioEmpresa)
        {
            _ServicioUsuario = servicioUsuario;
            _ServicioEmpresa = servicioEmpresa;
        }
        public IActionResult Modify()
        {
            return View();
        }
        public IActionResult Cancelar()
        {
            return RedirectToAction("Eventos", "Home");
        }
        public async Task<IActionResult> ModificarUsuario(Usuario usuario)
        {

            //Comprobar que los campos no esten vacios
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Direccion) ||
                string.IsNullOrWhiteSpace(usuario.Mail) || string.IsNullOrWhiteSpace(usuario.Pass) ||
                string.IsNullOrWhiteSpace(usuario.ConfirmPass))
            {
                ViewBag.Alert = "Por favor, complete todos los campos.";
                return View("Modify");
            }

            //Comprobar que todos los datos tengan un formato valido
            if (!ValidarRequisitosNombre(usuario.Nombre) || !ValidarRequisitosDireccion(usuario.Direccion) ||
                !ValidarRequisitosEmail(usuario.Mail) || !ValidarRequisitosPassword(usuario.Pass) || usuario.Pass != usuario.ConfirmPass)
            {
                ViewBag.Alert = "Alguno de los campos no cumple con los requisitos.";
                return View("Modify");
            }
            //Comprobar si el usuario en sessión es una cuenta de empresa o una cuenta de usuario
            if (User.IsInRole("Empresa"))
            {
                Empresa oEmpresa = new Empresa();
                oEmpresa.Mail = User.FindFirstValue(ClaimTypes.Email);
                _ServicioEmpresa.UpdateEmpresa(await _ServicioEmpresa.GetEmpresa(oEmpresa));
                
            }
            else 
            {
                Usuario oUsuario = await _ServicioUsuario.GetUsuario(usuario);
                usuario.IdUsuario = oUsuario.IdUsuario;
                _ServicioUsuario.UpdateUsuario(usuario);
            }


            return RedirectToAction("Eventos", "Home");
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
            if (nombre.Length < 6 || nombre.Length > 20)
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
    }
}
