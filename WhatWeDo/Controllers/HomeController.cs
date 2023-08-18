using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WhatWeDo.Models;

namespace WhatWeDo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Eventos ()
        {
            Evento oEvento = new Evento()
            {
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cc/Burger_King_2020.svg/150px-Burger_King_2020.svg.png"

               ,
                Titulo = "Burger King: Comer en compañia"

               ,
                Descripcion = "No sabes que hacer para comer, ven con tu familia o amigos! \n " +
                             "Obtén un descuento del 20% en el total de la cuenta por venir acompañado de 5 personas"
            };

            Evento oEvento2 = new Evento()
            {
                Imagen = "https://rubricadigital.es/wp-content/uploads/2022/01/logo-Ibai.jpg"

               ,
                Titulo = "Telepizza: Comer en compañia"

               ,
                Descripcion = "No sabes que hacer para comer, ven con tu familia o amigos! \n " +
                             "Obten un descuento del 20% en el total de la cuenta por venir acompañado de 5 personas"
            };

            Evento oEvento3 = new Evento()
            {
                Imagen = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRHQtU7j0ZHLqIisJkYEuLrwExbT1bMOw9XO5KwGW8N39JXqc7uh0lntOzs3l9qIHYnPQ0&usqp=CAU"

               ,
                Titulo = "McDonals: Comer en compañia"

               ,
                Descripcion = "No sabes que hacer para comer, ven con tu familia o amigos! \n " +
                             "Obten un descuento del 20% en el total de la cuenta por venir acompañado de 5 personas"
            };

            List<Evento> lstEventos = new List<Evento>
            {
                oEvento,
                oEvento2,
                oEvento3
            };

            return View(lstEventos);
        }

        [Authorize(Roles = "Empresa")]
        public IActionResult MisEventos()
        {
            Usuario usuario = new Usuario();
            usuario.Nombre = User.FindFirstValue(ClaimTypes.Name);
            usuario.Mail = User.FindFirstValue(ClaimTypes.Email);
            return View(usuario);
        }

        [Authorize(Roles = "Empresa")]
        public IActionResult CrearEvento()
        {
            return View();
        }
        
        [Authorize(Roles = "Empresa")]
        public IActionResult InsertEvento()
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