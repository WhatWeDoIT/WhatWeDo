using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WhatWeDo.Models;
using WhatWeDo.Servicios.Contratos;

namespace WhatWeDo.Controllers
{
    public class EventosEmpresaController : Controller
    {
        private readonly IEventoService _ServicioEvento;
        private readonly IUbicacionService _ServicioUbicacion;
        private readonly ICategoriaService _ServicioCategoria;
        private readonly IDescuentoService _ServicioDescuento;

        static List<Evento> lstEventosCategorizados = new List<Evento>();
        static bool bBuscarPorCategoria = false;

        public EventosEmpresaController(IEventoService servicioEvento
                            , IUbicacionService servicioUbicacion
                            , ICategoriaService servicioCategoria
                            , IDescuentoService servicioDescuento)
        {
            _ServicioEvento = servicioEvento;
            _ServicioUbicacion = servicioUbicacion;
            _ServicioCategoria = servicioCategoria;
            _ServicioDescuento = servicioDescuento;
        }
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> MisEventosEmpresa()
        {
            List<Evento> lstEventos = new List<Evento>();

            //Para redirigir a los eventos por categoria
            if (bBuscarPorCategoria)
            {
                lstEventos = lstEventosCategorizados;
                bBuscarPorCategoria = false;
            }
            else
            {
                lstEventos = await _ServicioEvento.GetEventosPorEmpresa(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            return View(lstEventos);
        }

        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> EventosPorCategoria(int categoria)
        {
            bBuscarPorCategoria = true;
            lstEventosCategorizados = await _ServicioEvento.GetEventosPorEmpresaCategoria(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), categoria);

            return RedirectToAction("MisEventosEmpresa", "EventosEmpresa");
        }

        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> CrearEvento()
        {
            //Devuelve la vista del formulario
            Evento oEvento = new Evento();
            oEvento.lstUbicaciones = await _ServicioUbicacion.GetUbicacionPorEmpresa(User.FindFirstValue(ClaimTypes.Email));
            ViewBag.direccion = new SelectList(oEvento.lstUbicaciones, "IdUbicacion", "Direccion");

            oEvento.lstCategorias = await _ServicioCategoria.GetCategorias();
            ViewBag.categoria = new SelectList(oEvento.lstCategorias, "IdCategoria", "Nombre");

            oEvento.lstDescuento = await _ServicioDescuento.GetDescuentos();
            ViewBag.descuento = new SelectList(oEvento.lstDescuento, "IdDescuento", "MostrarValor");

            return View();
        }

        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> InsertEvento(Evento oEvento, IFormFile Imagen
                                                    , string categoria, string direccion
                                                    , string descuento)
        {
            //Accion de crear evento
            Stream imagen = Imagen.OpenReadStream();
            string sUrlImagen = await SubirImagen(imagen, Imagen.FileName);
            oEvento.Imagen = sUrlImagen;
            oEvento.Titulo = User.FindFirstValue(ClaimTypes.Name) + " - " + oEvento.Titulo;
            oEvento.PlazasActuales = 0;
            oEvento.FkIdCategoria = Convert.ToInt32(categoria);
            oEvento.FkIdUbicacion = Convert.ToInt32(direccion);
            oEvento.FkIdDescuento = Convert.ToInt32(descuento);

            Random r = new Random();
            oEvento.ValorEnPuntos = r.Next(1, 10);


            await _ServicioEvento.InsertEvento(oEvento, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return RedirectToAction("MisEventosEmpresa", "EventosEmpresa");
        }

        [Authorize(Roles = "Empresa")]
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
    }
}
