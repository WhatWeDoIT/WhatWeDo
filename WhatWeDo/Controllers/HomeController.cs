using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WhatWeDo.Models;
using Firebase.Auth;
using Firebase.Storage;
using System.Data.SqlClient;
using WhatWeDo.Servicios.Contratos;

namespace WhatWeDo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventoService _ServicioEvento;

        public HomeController(IEventoService servicioEvento)
        {
            _ServicioEvento = servicioEvento;
        }

        public async Task <IActionResult> Eventos (List<Evento> lstEventos)
        {
            lstEventos = await _ServicioEvento.GetEventos();

            return View(lstEventos);
        }

        [Authorize(Roles = "Empresa")]
        public IActionResult MisEventos()
        {
            Usuario usuario = new Usuario();
            usuario.Nombre = User.FindFirstValue(ClaimTypes.Name);            
            return View(usuario);
        }

        [Authorize(Roles = "Empresa")]
        public IActionResult CrearEvento()
        {
            //Devuelve la vista del formulario
            return View();
        }
        
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> InsertEvento(Evento oEvento, IFormFile Imagen)
        {
            //Accion de crear evento
            Stream imagen = Imagen.OpenReadStream();
            string sUrlImagen = await SubirImagen(imagen, Imagen.FileName);
            oEvento.Imagen = sUrlImagen;
            oEvento.PlazasActuales = oEvento.PlazasMaximas;


            await _ServicioEvento.InsertEvento(oEvento);

            return RedirectToAction("Eventos", "Home");
        }

        public async Task<string> SubirImagen(Stream archivo, string nombre)
        {
            //credenciales firebase
            string sEmail = "whatwedo@gmail.com";
            string sClave = "123454321";
            string sRuta = "whatwedoimgs.appspot.com";
            string sApiKey = "AIzaSyClTyWsO6enTmcRuElffSSUYzHYcpQuCP8";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(sApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(sEmail, sClave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                sRuta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true

                })
                .Child("Fotos_Evento")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);

            var downloadURL = await task;

            return downloadURL;

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}