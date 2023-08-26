using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WhatWeDo.Models;
using WhatWeDo.Servicios.Contratos;
using System.Drawing;

namespace WhatWeDo.Controllers
{
    public class EventosEmpresaController : Controller
    {
        private readonly IEventoService _ServicioEvento;
        private readonly IUbicacionService _ServicioUbicacion;
        private readonly ICategoriaService _ServicioCategoria;
        private readonly IDescuentoService _ServicioDescuento;
        private readonly IEmpresaService _ServicioEmpresa;

        static List<Evento> lstEventosCategorizados = new List<Evento>();
        static bool bBuscarPorCategoria = false;

        private const int PageSize = 3; // Número de eventos por página

        public EventosEmpresaController(IEventoService servicioEvento
                            , IUbicacionService servicioUbicacion
                            , ICategoriaService servicioCategoria
                            , IDescuentoService servicioDescuento
                            , IEmpresaService servicioEmpresa)
        {
            _ServicioEvento = servicioEvento;
            _ServicioUbicacion = servicioUbicacion;
            _ServicioCategoria = servicioCategoria;
            _ServicioDescuento = servicioDescuento;
            _ServicioEmpresa = servicioEmpresa;
        }
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> MisEventosEmpresa(int page = 1)
        {
            List<Evento> lstEventos = new List<Evento>();            

            //Para redirigir a los eventos por categoria
            if (bBuscarPorCategoria)
            {
                lstEventos = lstEventosCategorizados;
                string categoriaSeleccionada = TempData["CategoriaSeleccionada"] as string;

                if (categoriaSeleccionada != null)
                {
                    ViewData["CetegoriaRecibida"] = categoriaSeleccionada;
                }
                bBuscarPorCategoria = false;
            }
            else
            {
                lstEventos = await _ServicioEvento.GetEventosPorEmpresa(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            // Implementar la paginación
            int totalEventos = lstEventos.Count;
            int totalPages = (int)Math.Ceiling((double)totalEventos / PageSize);
            var paginatedEvents = lstEventos.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.PaginaActual = page;
            ViewBag.TotalPaginas = totalPages;

            //actualizamos el saldo de la empresa
            if (User.IsInRole("Empresa"))
            {
                Empresa oEmpresa = await _ServicioEmpresa.GetEmpresa(User.FindFirstValue(ClaimTypes.Email));

                await ActualizarSaldoEmpresa(oEmpresa);
            }

            return View(paginatedEvents);
        }

        public async Task ActualizarSaldoEmpresa(Empresa empresa)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            string rol = "Empresa";

            Empresa oEmpresa = await _ServicioEmpresa.GetEmpresa(empresa.Mail);
            empresa.Nombre = oEmpresa.Nombre;
            empresa.IdEmpresa = oEmpresa.IdEmpresa;
            empresa.Saldo = oEmpresa.Saldo;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, empresa.Nombre),
                new Claim(ClaimTypes.Email, empresa.Mail),
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.NameIdentifier, empresa.IdEmpresa.ToString()),
                new Claim("saldo", empresa.Saldo.ToString() + " €")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));            
        }

        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> EventosPorCategoria(int categoria)
        {
            bBuscarPorCategoria = true;
            lstEventosCategorizados = await _ServicioEvento.GetEventosPorEmpresaCategoria(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), categoria);

            //Enviamos la cetegoria al controller siguiente
            TempData["CategoriaSeleccionada"] = categoria.ToString();

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


            //actualizamos el saldo de la empresa
            if (User.IsInRole("Empresa"))
            {
                Empresa oEmpresa = await _ServicioEmpresa.GetEmpresa(User.FindFirstValue(ClaimTypes.Email));

                await ActualizarSaldoEmpresa(oEmpresa);
            }

            return View();
        }

        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> ModificarEvento(int idEvento)
        {
            //Devuelve la vista del formulario
            Evento oEvento = new Evento();

            oEvento = await _ServicioEvento.GetEventoPorId(idEvento);

            oEvento.Titulo = oEvento.Titulo.Replace(User.FindFirstValue(ClaimTypes.Name) + " - ", "");

            oEvento.lstUbicaciones = await _ServicioUbicacion.GetUbicacionPorEmpresa(User.FindFirstValue(ClaimTypes.Email));
            ViewBag.direccion = new SelectList(oEvento.lstUbicaciones, "IdUbicacion", "Direccion", oEvento.FkIdUbicacion);

            oEvento.lstCategorias = await _ServicioCategoria.GetCategorias();
            ViewBag.categoria = new SelectList(oEvento.lstCategorias, "IdCategoria", "Nombre", oEvento.FkIdCategoria);

            oEvento.lstDescuento = await _ServicioDescuento.GetDescuentos();
            ViewBag.descuento = new SelectList(oEvento.lstDescuento, "IdDescuento", "MostrarValor", oEvento.FkIdDescuento);


            //actualizamos el saldo de la empresa
            if (User.IsInRole("Empresa"))
            {
                Empresa oEmpresa = await _ServicioEmpresa.GetEmpresa(User.FindFirstValue(ClaimTypes.Email));

                await ActualizarSaldoEmpresa(oEmpresa);
            }

            return View(oEvento);
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
        public async Task<IActionResult> UpdateEvento(Evento oEvento, IFormFile Imagen
                                                   , string categoria, string direccion
                                                   , string descuento)
        {
            //Accion de modificar evento
            if (Imagen != null)
            {
                Stream imagen = Imagen.OpenReadStream();
                string sUrlImagen = await SubirImagen(imagen, Imagen.FileName);
                oEvento.Imagen = sUrlImagen;
            }          
            oEvento.Titulo = User.FindFirstValue(ClaimTypes.Name) + " - " + oEvento.Titulo;
            oEvento.PlazasActuales = 0;
            oEvento.FkIdCategoria = Convert.ToInt32(categoria);
            oEvento.FkIdUbicacion = Convert.ToInt32(direccion);
            oEvento.FkIdDescuento = Convert.ToInt32(descuento);

            await _ServicioEvento.UpdateEvento(oEvento);

            return RedirectToAction("MisEventosEmpresa", "EventosEmpresa");
        }

        [Authorize(Roles = "Empresa")]
        public async Task<string> SubirImagen(Stream archivo, string nombre)
        {
            // Credenciales de Firebase
            string sEmail = "whatwedo@gmail.com";
            string sClave = "123454321";
            string sRuta = "whatwedoimgs.appspot.com";
            string sApiKey = "AIzaSyClTyWsO6enTmcRuElffSSUYzHYcpQuCP8";

            // Asignamos nombre aleatorio
            Random random = new Random();
            nombre = nombre.Trim() + random.Next(1, 100000).ToString();

            var auth = new FirebaseAuthProvider(new FirebaseConfig(sApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(sEmail, sClave);

            var cancellation = new CancellationTokenSource();

            // Leer la imagen del stream y redimensionarla
            using (var originalImage = Image.FromStream(archivo))
            using (var resizedImage = new Bitmap(626, 417))
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(originalImage, 0, 0, 626, 417);

                // Guardar la imagen redimensionada en un nuevo stream
                using (var resizedStream = new MemoryStream())
                {
                    resizedImage.Save(resizedStream, originalImage.RawFormat);
                    resizedStream.Seek(0, SeekOrigin.Begin);

                    var task = new FirebaseStorage(
                        sRuta,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true

                        })
                        .Child("Fotos_Evento")
                        .Child(nombre)
                        .PutAsync(resizedStream, cancellation.Token);

                    var downloadURL = await task;

                    return downloadURL;
                }
            }
        }
    }
}
