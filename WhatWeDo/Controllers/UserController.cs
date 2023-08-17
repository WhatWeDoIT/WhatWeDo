using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using WhatWeDo.Models;
using WhatWeDo.Recursos;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Servicios.Implementacion;

namespace WhatWeDo.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsuarioService _ServicioUsuario;
        private readonly IEmpresaService _ServicioEmpresa;

        public UserController(IUsuarioService servicioUsuario, IEmpresaService servicioEmpresa)
        {
            _ServicioUsuario = servicioUsuario;
            _ServicioEmpresa = servicioEmpresa;
        }
        public async Task<IActionResult> MisDatosUsuario(Usuario usuario)
        {
            usuario.Mail = User.FindFirstValue(ClaimTypes.Email);
            await _ServicioUsuario.GetUsuario(usuario);

            return View(usuario);
        }

        public async Task<IActionResult> MisDatosEmpresa(Empresa empresa)
        {
            empresa.Mail = User.FindFirstValue(ClaimTypes.Email);
            await _ServicioEmpresa.GetEmpresa(empresa);

            return View(empresa);
        }

        public async Task<IActionResult> ModificarUsuario(Usuario usuario)
        {
            //Comprobar que los campos no esten vacios
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || 
                string.IsNullOrWhiteSpace(usuario.Mail) || string.IsNullOrWhiteSpace(usuario.Pass) ||
                string.IsNullOrWhiteSpace(usuario.ConfirmPass))
            {
                ViewBag.Alert = "Por favor, complete todos los campos.";
                return View("MisDatosUsuario",usuario);
            }

            //Comprobar que todos los datos tengan un formato valido
            if (!ValidarRequisitosNombre(usuario.Nombre) ||
                !ValidarRequisitosEmail(usuario.Mail) || !ValidarRequisitosPassword(usuario.Pass) || usuario.Pass != usuario.ConfirmPass)
            {
                ViewBag.Alert = "Alguno de los campos no cumple con los requisitos.";
                return View("MisDatosUsuario", usuario);
            }
            usuario.Pass = Utilidades.EncriptarPassword(usuario.Pass); //Encriptar contraseña
            await _ServicioUsuario.UpdateUsuario(usuario);           
            
            return RedirectToAction("Eventos", "Home");
        }

        public async Task<IActionResult> ModificarEmpresa(Empresa empresa)
        {
            //Comprobar que los campos no esten vacios
            if (string.IsNullOrWhiteSpace(empresa.Nombre) || string.IsNullOrWhiteSpace(empresa.Direccion) ||
                string.IsNullOrWhiteSpace(empresa.Mail) || string.IsNullOrWhiteSpace(empresa.Pass) ||
                string.IsNullOrWhiteSpace(empresa.ConfirmPass))
            {
                ViewBag.Alert = "Por favor, complete todos los campos.";
                return View("MisDatosEmpresa", empresa);
            }

            //Comprobar que todos los datos tengan un formato valido
            if (!ValidarRequisitosNombre(empresa.Nombre) || !ValidarRequisitosDireccion(empresa.Direccion) ||
                !ValidarRequisitosEmail(empresa.Mail) || !ValidarRequisitosPassword(empresa.Pass) || empresa.Pass != empresa.ConfirmPass)
            {
                ViewBag.Alert = "Alguno de los campos no cumple con los requisitos.";
                return View("MisDatosEmpresa", empresa);
            }
            empresa.Pass = Utilidades.EncriptarPassword(empresa.Pass); //Encriptar contraseña
            await _ServicioEmpresa.UpdateEmpresa(empresa);

            return RedirectToAction("Eventos", "Home");
        }

        public IActionResult Cancelar()
        {
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
    }
}
