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
                ImgSource = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cc/Burger_King_2020.svg/150px-Burger_King_2020.svg.png"

               ,
                TituloEvento = "Burger King: Comer en compañia"

               ,
                DescEvento = "No sabes que hacer para comer, ven con tu familia o amigos! \n " +
                             "Obtén un descuento del 20% en el total de la cuenta por venir acompañado de 5 personas"
            };

            Evento oEvento2 = new Evento()
            {
                ImgSource = "https://rubricadigital.es/wp-content/uploads/2022/01/logo-Ibai.jpg"

               ,
                TituloEvento = "Telepizza: Comer en compañia"

               ,
                DescEvento = "No sabes que hacer para comer, ven con tu familia o amigos! \n " +
                             "Obten un descuento del 20% en el total de la cuenta por venir acompañado de 5 personas"
            };

            Evento oEvento3 = new Evento()
            {
                ImgSource = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRHQtU7j0ZHLqIisJkYEuLrwExbT1bMOw9XO5KwGW8N39JXqc7uh0lntOzs3l9qIHYnPQ0&usqp=CAU"

               ,
                TituloEvento = "McDonals: Comer en compañia"

               ,
                DescEvento = "No sabes que hacer para comer, ven con tu familia o amigos! \n " +
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