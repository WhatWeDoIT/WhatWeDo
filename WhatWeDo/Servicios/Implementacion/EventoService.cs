using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using WhatWeDo.Servicios.Configuracion;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Models;
using System.Data.SqlTypes;
using Firebase.Auth;
using System.Security.Claims;
using WhatWeDo.Recursos;
using static WhatWeDo.Recursos.CargasCombo;

namespace WhatWeDo.Servicios.Implementacion
{
    public class EventoService : IEventoService
    {
        private readonly ConfiguracionConexion _conexion;
        private readonly ICategoriaService _ServicioCategoria;
        private readonly IUbicacionService _ServicioUbicacion;

        public EventoService(IOptions<ConfiguracionConexion> conexion
                            , ICategoriaService servicioCategoria
                            , IUbicacionService servicioUbicacion)
        {
            _conexion = conexion.Value;
            _ServicioCategoria = servicioCategoria;
            _ServicioUbicacion = servicioUbicacion;
        }
          
        public async Task<List<Evento>> GetEventos (int idUsuario)
        {
            List<Evento> lstEventos = new List<Evento>();
            
            try
            {                
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEventos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Evento oEvento = new Evento();
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            oEvento.Titulo = dr["Titulo"].ToString();
                            oEvento.Descripcion = dr["Descripcion"].ToString();
                            oEvento.FechaInicio = (DateTime)dr["FechaInicio"];
                            oEvento.FechaFin = (DateTime)dr["FechaFin"]; 
                            oEvento.PlazasActuales = Convert.ToInt32(dr["PlazasActuales"]);
                            oEvento.PlazasMaximas = Convert.ToInt32(dr["PlazasMaximas"]);
                            oEvento.Precio = double.Parse(dr["Precio"].ToString());
                            oEvento.Imagen = dr["Imagen"].ToString();
                            if (!string.IsNullOrWhiteSpace(dr["FkIdPunto"].ToString()))
                                oEvento.FkIdPunto = Convert.ToInt32(dr["FkIdPunto"]);
                                              
                            if (!string.IsNullOrWhiteSpace(dr["FkIdDescuento"].ToString()))
                                oEvento.FkIdDescuento = Convert.ToInt32(dr["FkIdDescuento"]);
                            oEvento.ValorEnPuntos = Convert.ToInt32(dr["ValorEnPuntos"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdCategoria"].ToString()))
                            {
                                oEvento.FkIdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);
                                //Obtenemos tambien el nombre de la categoria
                                Categoria oCategoria = new Categoria();
                                oCategoria.IdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);

                                oCategoria = await _ServicioCategoria.GetCategoria(oCategoria);
                                oEvento.CategoriaNombre = oCategoria.Nombre;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["FkIdUbicacion"].ToString()))
                            {
                                oEvento.FkIdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);
                                //Obtenemos tambien el nombre de la de la ubicacion
                                Ubicacion oUbicacion = new Ubicacion();
                                oUbicacion.IdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);

                                oUbicacion = await _ServicioUbicacion.GetUbicacion(oUbicacion);
                                oEvento.UbicacionNombre = oUbicacion.Direccion;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["HoraInicio"].ToString()))
                            {
                                oEvento.HoraIncio = (TimeSpan)dr["HoraInicio"];
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraFin"].ToString()))
                            {
                                oEvento.HoraFin = (TimeSpan)dr["HoraFin"];
                            }                            

                            oEvento.Reservado = await GetReservado(idUsuario, oEvento.IdEvento);

                            lstEventos.Add(oEvento);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstEventos;
        }

        public async Task<Evento> GetEventoPorId(int idEvento)
        {
           Evento oEvento = new Evento ();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEventoPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEvento", idEvento));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            oEvento.Titulo = dr["Titulo"].ToString();
                            oEvento.Descripcion = dr["Descripcion"].ToString();
                            oEvento.FechaInicio = (DateTime)dr["FechaInicio"];
                            oEvento.FechaFin = (DateTime)dr["FechaFin"];
                            oEvento.PlazasActuales = Convert.ToInt32(dr["PlazasActuales"]);
                            oEvento.PlazasMaximas = Convert.ToInt32(dr["PlazasMaximas"]);
                            oEvento.Precio = double.Parse(dr["Precio"].ToString());
                            oEvento.Imagen = dr["Imagen"].ToString();
                            if (!string.IsNullOrWhiteSpace(dr["FkIdPunto"].ToString()))
                                oEvento.FkIdPunto = Convert.ToInt32(dr["FkIdPunto"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdDescuento"].ToString()))
                                oEvento.FkIdDescuento = Convert.ToInt32(dr["FkIdDescuento"]);
                            oEvento.ValorEnPuntos = Convert.ToInt32(dr["ValorEnPuntos"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdCategoria"].ToString()))
                            {
                                oEvento.FkIdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);
                                //Obtenemos tambien el nombre de la categoria
                                Categoria oCategoria = new Categoria();
                                oCategoria.IdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);

                                oCategoria = await _ServicioCategoria.GetCategoria(oCategoria);
                                oEvento.CategoriaNombre = oCategoria.Nombre;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["FkIdUbicacion"].ToString()))
                            {
                                oEvento.FkIdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);
                                //Obtenemos tambien el nombre de la de la ubicacion
                                Ubicacion oUbicacion = new Ubicacion();
                                oUbicacion.IdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);

                                oUbicacion = await _ServicioUbicacion.GetUbicacion(oUbicacion);
                                oEvento.UbicacionNombre = oUbicacion.Direccion;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["HoraInicio"].ToString()))
                            {
                                oEvento.HoraIncio = (TimeSpan)dr["HoraInicio"];
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraFin"].ToString()))
                            {
                                oEvento.HoraFin = (TimeSpan)dr["HoraFin"];
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oEvento;
        }

        public async Task<EventoPago> GetEventoPago(int idEvento, int idUsuario)
        {
            EventoPago oEventoPago = new EventoPago();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEventoPago", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEvento", idEvento));
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oEventoPago.IdEventoPago = Convert.ToInt32(dr["IdEventoPago"]);
                            oEventoPago.FkIdEvento = Convert.ToInt32(dr["FkIdEvento"]);
                            oEventoPago.FkIdUsuario = Convert.ToInt32(dr["FkIdUsuario"]);
                            oEventoPago.FkIdEmpresa = Convert.ToInt32(dr["FkIdEmpresa"]);
                            oEventoPago.FechaAsistencia = (DateTime)dr["FechaAsistencia"];
                            oEventoPago.Miembros = Convert.ToInt32(dr["Miembros"]);
                            oEventoPago.PrecioTotal = double.Parse(dr["PrecioTotal"].ToString());
                            oEventoPago.PuntosAsignados = Convert.ToInt32(dr["PuntosAsignados"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oEventoPago;
        }

        public async Task<List<Evento>> GetEventosPorCategoria(int nCategoria)
        {
            List<Evento> lstEventos = new List<Evento>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEventosPorCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FkIdCategoria", nCategoria));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Evento oEvento = new Evento();
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            oEvento.Titulo = dr["Titulo"].ToString();
                            oEvento.Descripcion = dr["Descripcion"].ToString();
                            oEvento.FechaInicio = (DateTime)dr["FechaInicio"];
                            oEvento.FechaFin = (DateTime)dr["FechaFin"];
                            oEvento.PlazasActuales = Convert.ToInt32(dr["PlazasActuales"]);
                            oEvento.PlazasMaximas = Convert.ToInt32(dr["PlazasMaximas"]);
                            oEvento.Precio = double.Parse(dr["Precio"].ToString());
                            oEvento.Imagen = dr["Imagen"].ToString();
                            if (!string.IsNullOrWhiteSpace(dr["FkIdPunto"].ToString()))
                                oEvento.FkIdPunto = Convert.ToInt32(dr["FkIdPunto"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdDescuento"].ToString()))
                                oEvento.FkIdDescuento = Convert.ToInt32(dr["FkIdDescuento"]);
                            oEvento.ValorEnPuntos = Convert.ToInt32(dr["ValorEnPuntos"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdCategoria"].ToString()))
                            {
                                oEvento.FkIdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);
                                //Obtenemos tambien el nombre de la categoria
                                Categoria oCategoria = new Categoria();
                                oCategoria.IdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);

                                oCategoria = await _ServicioCategoria.GetCategoria(oCategoria);
                                oEvento.CategoriaNombre = oCategoria.Nombre;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["FkIdUbicacion"].ToString()))
                            {
                                oEvento.FkIdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);
                                //Obtenemos tambien el nombre de la de la ubicacion
                                Ubicacion oUbicacion = new Ubicacion();
                                oUbicacion.IdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);

                                oUbicacion = await _ServicioUbicacion.GetUbicacion(oUbicacion);
                                oEvento.UbicacionNombre = oUbicacion.Direccion;
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraInicio"].ToString()))
                            {
                                oEvento.HoraIncio = (TimeSpan)dr["HoraInicio"];
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraFin"].ToString()))
                            {
                                oEvento.HoraFin = (TimeSpan)dr["HoraFin"];
                            }

                            lstEventos.Add(oEvento);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstEventos;
        }

        public async Task<List<Evento>> GetEventosPorUsuario(int idUsuario)
        {
            List<Evento> lstEventos = new List<Evento>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUsuarioEventosPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Evento oEvento = new Evento();
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            oEvento.Titulo = dr["Titulo"].ToString();
                            oEvento.Descripcion = dr["Descripcion"].ToString();
                            oEvento.FechaInicio = (DateTime)dr["FechaInicio"];
                            oEvento.FechaFin = (DateTime)dr["FechaFin"];
                            oEvento.PlazasActuales = Convert.ToInt32(dr["PlazasActuales"]);
                            oEvento.PlazasMaximas = Convert.ToInt32(dr["PlazasMaximas"]);
                            oEvento.Precio = double.Parse(dr["Precio"].ToString());
                            oEvento.Imagen = dr["Imagen"].ToString();
                            if (!string.IsNullOrWhiteSpace(dr["FkIdPunto"].ToString()))
                                oEvento.FkIdPunto = Convert.ToInt32(dr["FkIdPunto"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdDescuento"].ToString()))
                                oEvento.FkIdDescuento = Convert.ToInt32(dr["FkIdDescuento"]);
                            oEvento.ValorEnPuntos = Convert.ToInt32(dr["ValorEnPuntos"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdCategoria"].ToString()))
                            {
                                oEvento.FkIdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);
                                //Obtenemos tambien el nombre de la categoria
                                Categoria oCategoria = new Categoria();
                                oCategoria.IdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);

                                oCategoria = await _ServicioCategoria.GetCategoria(oCategoria);
                                oEvento.CategoriaNombre = oCategoria.Nombre;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["FkIdUbicacion"].ToString()))
                            {
                                oEvento.FkIdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);
                                //Obtenemos tambien el nombre de la de la ubicacion
                                Ubicacion oUbicacion = new Ubicacion();
                                oUbicacion.IdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);

                                oUbicacion = await _ServicioUbicacion.GetUbicacion(oUbicacion);
                                oEvento.UbicacionNombre = oUbicacion.Direccion;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["HoraInicio"].ToString()))
                            {
                                oEvento.HoraIncio = (TimeSpan)dr["HoraInicio"];
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraFin"].ToString()))
                            {
                                oEvento.HoraFin = (TimeSpan)dr["HoraFin"];
                            }

                            lstEventos.Add(oEvento);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstEventos;
        }

        public async Task<List<Evento>> GetEventosPorUsuarioCategoria(int idUsuario, int categoria)
        {
            List<Evento> lstEventos = new List<Evento>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUsuarioEventosPorIdyCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("@IdCategoria", categoria));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Evento oEvento = new Evento();
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            oEvento.Titulo = dr["Titulo"].ToString();
                            oEvento.Descripcion = dr["Descripcion"].ToString();
                            oEvento.FechaInicio = (DateTime)dr["FechaInicio"];
                            oEvento.FechaFin = (DateTime)dr["FechaFin"];
                            oEvento.PlazasActuales = Convert.ToInt32(dr["PlazasActuales"]);
                            oEvento.PlazasMaximas = Convert.ToInt32(dr["PlazasMaximas"]);
                            oEvento.Precio = double.Parse(dr["Precio"].ToString());
                            oEvento.Imagen = dr["Imagen"].ToString();
                            if (!string.IsNullOrWhiteSpace(dr["FkIdPunto"].ToString()))
                                oEvento.FkIdPunto = Convert.ToInt32(dr["FkIdPunto"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdDescuento"].ToString()))
                                oEvento.FkIdDescuento = Convert.ToInt32(dr["FkIdDescuento"]);
                            oEvento.ValorEnPuntos = Convert.ToInt32(dr["ValorEnPuntos"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdCategoria"].ToString()))
                            {
                                oEvento.FkIdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);
                                //Obtenemos tambien el nombre de la categoria
                                Categoria oCategoria = new Categoria();
                                oCategoria.IdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);

                                oCategoria = await _ServicioCategoria.GetCategoria(oCategoria);
                                oEvento.CategoriaNombre = oCategoria.Nombre;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["FkIdUbicacion"].ToString()))
                            {
                                oEvento.FkIdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);
                                //Obtenemos tambien el nombre de la de la ubicacion
                                Ubicacion oUbicacion = new Ubicacion();
                                oUbicacion.IdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);

                                oUbicacion = await _ServicioUbicacion.GetUbicacion(oUbicacion);
                                oEvento.UbicacionNombre = oUbicacion.Direccion;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["HoraInicio"].ToString()))
                            {
                                oEvento.HoraIncio = (TimeSpan)dr["HoraInicio"];
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraFin"].ToString()))
                            {
                                oEvento.HoraFin = (TimeSpan)dr["HoraFin"];
                            }


                            lstEventos.Add(oEvento);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstEventos;
        }

        public async Task<List<Evento>> GetEventosPorEmpresa(int idEmpresa)
        {
            List<Evento> lstEventos = new List<Evento>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEmpresaEventosPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpresa", idEmpresa));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Evento oEvento = new Evento();
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            oEvento.Titulo = dr["Titulo"].ToString();
                            oEvento.Descripcion = dr["Descripcion"].ToString();
                            oEvento.FechaInicio = (DateTime)dr["FechaInicio"];
                            oEvento.FechaFin = (DateTime)dr["FechaFin"];
                            oEvento.PlazasActuales = Convert.ToInt32(dr["PlazasActuales"]);
                            oEvento.PlazasMaximas = Convert.ToInt32(dr["PlazasMaximas"]);
                            oEvento.Precio = double.Parse(dr["Precio"].ToString());
                            oEvento.Imagen = dr["Imagen"].ToString();
                            if (!string.IsNullOrWhiteSpace(dr["FkIdPunto"].ToString()))
                                oEvento.FkIdPunto = Convert.ToInt32(dr["FkIdPunto"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdDescuento"].ToString()))
                                oEvento.FkIdDescuento = Convert.ToInt32(dr["FkIdDescuento"]);
                            oEvento.ValorEnPuntos = Convert.ToInt32(dr["ValorEnPuntos"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdCategoria"].ToString()))
                            {
                                oEvento.FkIdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);
                                //Obtenemos tambien el nombre de la categoria
                                Categoria oCategoria = new Categoria();
                                oCategoria.IdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);

                                oCategoria = await _ServicioCategoria.GetCategoria(oCategoria);
                                oEvento.CategoriaNombre = oCategoria.Nombre;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["FkIdUbicacion"].ToString()))
                            {
                                oEvento.FkIdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);
                                //Obtenemos tambien el nombre de la de la ubicacion
                                Ubicacion oUbicacion = new Ubicacion();
                                oUbicacion.IdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);

                                oUbicacion = await _ServicioUbicacion.GetUbicacion(oUbicacion);
                                oEvento.UbicacionNombre = oUbicacion.Direccion;
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraInicio"].ToString()))
                            {
                                oEvento.HoraIncio = (TimeSpan)dr["HoraInicio"];
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraFin"].ToString()))
                            {
                                oEvento.HoraFin = (TimeSpan)dr["HoraFin"];
                            }

                            lstEventos.Add(oEvento);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstEventos;
        }

        public async Task<List<Evento>> GetEventosPorEmpresaCategoria(int idEmpresa, int categoria)
        {
            List<Evento> lstEventos = new List<Evento>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEmpresaEventosPorIdyCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpresa", idEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@IdCategoria", categoria));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Evento oEvento = new Evento();
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            oEvento.Titulo = dr["Titulo"].ToString();
                            oEvento.Descripcion = dr["Descripcion"].ToString();
                            oEvento.FechaInicio = (DateTime)dr["FechaInicio"];
                            oEvento.FechaFin = (DateTime)dr["FechaFin"];
                            oEvento.PlazasActuales = Convert.ToInt32(dr["PlazasActuales"]);
                            oEvento.PlazasMaximas = Convert.ToInt32(dr["PlazasMaximas"]);
                            oEvento.Precio = double.Parse(dr["Precio"].ToString());
                            oEvento.Imagen = dr["Imagen"].ToString();
                            if (!string.IsNullOrWhiteSpace(dr["FkIdPunto"].ToString()))
                                oEvento.FkIdPunto = Convert.ToInt32(dr["FkIdPunto"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdDescuento"].ToString()))
                                oEvento.FkIdDescuento = Convert.ToInt32(dr["FkIdDescuento"]);
                            oEvento.ValorEnPuntos = Convert.ToInt32(dr["ValorEnPuntos"]);

                            if (!string.IsNullOrWhiteSpace(dr["FkIdCategoria"].ToString()))
                            {
                                oEvento.FkIdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);
                                //Obtenemos tambien el nombre de la categoria
                                Categoria oCategoria = new Categoria();
                                oCategoria.IdCategoria = Convert.ToInt32(dr["FkIdCategoria"]);

                                oCategoria = await _ServicioCategoria.GetCategoria(oCategoria);
                                oEvento.CategoriaNombre = oCategoria.Nombre;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["FkIdUbicacion"].ToString()))
                            {
                                oEvento.FkIdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);
                                //Obtenemos tambien el nombre de la de la ubicacion
                                Ubicacion oUbicacion = new Ubicacion();
                                oUbicacion.IdUbicacion = Convert.ToInt32(dr["FkIdUbicacion"]);

                                oUbicacion = await _ServicioUbicacion.GetUbicacion(oUbicacion);
                                oEvento.UbicacionNombre = oUbicacion.Direccion;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["HoraInicio"].ToString()))
                            {
                                oEvento.HoraIncio = (TimeSpan)dr["HoraInicio"];
                            }

                            if (!string.IsNullOrWhiteSpace(dr["HoraFin"].ToString()))
                            {
                                oEvento.HoraFin = (TimeSpan)dr["HoraFin"];
                            }

                            lstEventos.Add(oEvento);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstEventos;
        }
    
        public async Task InsertEvento(Evento oEvento, int idEmpresa)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Titulo", oEvento.Titulo));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", oEvento.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", oEvento.FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFin", oEvento.FechaFin));
                    cmd.Parameters.Add(new SqlParameter("@PlazasActuales", oEvento.PlazasActuales));
                    cmd.Parameters.Add(new SqlParameter("@PlazasMaximas", oEvento.PlazasMaximas));
                    cmd.Parameters.Add(new SqlParameter("@Precio", oEvento.Precio));
                    cmd.Parameters.Add(new SqlParameter("@Imagen", oEvento.Imagen));
                    cmd.Parameters.Add(new SqlParameter("@FkIdPunto", oEvento.FkIdPunto == null ? (object)DBNull.Value : oEvento.FkIdPunto));
                    cmd.Parameters.Add(new SqlParameter("@FkIdUbicacion", oEvento.FkIdUbicacion == null ? (object)DBNull.Value : oEvento.FkIdUbicacion));
                    cmd.Parameters.Add(new SqlParameter("@FkIdCategoria", oEvento.FkIdCategoria == null ? (object)DBNull.Value : oEvento.FkIdCategoria));
                    cmd.Parameters.Add(new SqlParameter("@FkIdDescuento", oEvento.FkIdDescuento == null ? (object)DBNull.Value : oEvento.FkIdDescuento));
                    cmd.Parameters.Add(new SqlParameter("@ValorEnPuntos", oEvento.ValorEnPuntos));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpresa", idEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@HoraInicio", oEvento.HoraIncio));
                    cmd.Parameters.Add(new SqlParameter("@HoraFin", oEvento.HoraFin));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }

        public async Task InsertEventoPago(EventoPago oEventoPago)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertEventoPago", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FechaAsistencia", oEventoPago.FechaAsistencia));
                    cmd.Parameters.Add(new SqlParameter("@Miembros", oEventoPago.Miembros));
                    cmd.Parameters.Add(new SqlParameter("@PrecioTotal", oEventoPago.PrecioTotal));
                    cmd.Parameters.Add(new SqlParameter("@PuntosAsignados", oEventoPago.PuntosAsignados));
                    cmd.Parameters.Add(new SqlParameter("@FkIdEmpresa", oEventoPago.FkIdEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@FkIdUsuario", oEventoPago.FkIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("@FkIdEvento", oEventoPago.FkIdEvento));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateEvento(Evento oEvento)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Titulo", oEvento.Titulo));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", oEvento.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", oEvento.FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFin", oEvento.FechaFin));
                    cmd.Parameters.Add(new SqlParameter("@PlazasActuales", oEvento.PlazasActuales));
                    cmd.Parameters.Add(new SqlParameter("@PlazasMaximas", oEvento.PlazasMaximas));
                    cmd.Parameters.Add(new SqlParameter("@Precio", oEvento.Precio));
                    cmd.Parameters.Add(new SqlParameter("@Imagen", oEvento.Imagen));
                    cmd.Parameters.Add(new SqlParameter("@FkIdPunto", oEvento.FkIdPunto));
                    cmd.Parameters.Add(new SqlParameter("@FkIdUbicacion", oEvento.FkIdUbicacion));
                    cmd.Parameters.Add(new SqlParameter("@FkIdCategoria", oEvento.FkIdCategoria));
                    cmd.Parameters.Add(new SqlParameter("@FkIdDescuento", oEvento.FkIdDescuento));
                    cmd.Parameters.Add(new SqlParameter("@ValorEnPuntos", oEvento.ValorEnPuntos));
                    cmd.Parameters.Add(new SqlParameter("@HoraInicio", oEvento.HoraIncio));
                    cmd.Parameters.Add(new SqlParameter("@HoraFin", oEvento.HoraFin));
                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteEvento(Evento oEvento)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", oEvento.IdEvento));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> GetReservado(int idUsuario, int idEvento)
        {
            bool bReservado = false;
            Evento oEvento = new Evento();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUsuarioReserva", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("@IdEvento", idEvento));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oEvento.IdEvento = Convert.ToInt32(dr["IdEvento"]);
                            bReservado = true;
                        }                           
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return bReservado;
        }

        public async Task ReservarEvento (int idEvento, int idUsuario, DateTime fecha, int miembros)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertUsuarioEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FkIdUsuario", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("@FkIdEvento", idEvento));
                    cmd.Parameters.Add(new SqlParameter("@FechaPlan", fecha));
                   
                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                    Evento oEvento = await GetEventoPorId(idEvento);
                    await ModificarPlazasEvento(oEvento, true, miembros);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AnularReservaEvento(int idEvento, int idUsuario, int miembros)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteUsuarioEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FkIdUsuario", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("@FkIdEvento", idEvento));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                    Evento oEvento = await GetEventoPorId(idEvento);
                    await ModificarPlazasEvento(oEvento, false, miembros);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ModificarPlazasEvento(Evento oEvento, bool bReservar, int Miembros)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateEventoPlazas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (bReservar)
                    {
                        cmd.Parameters.Add(new SqlParameter("@PlazasActuales", oEvento.PlazasActuales + Miembros));
                        cmd.Parameters.Add(new SqlParameter("@PlazasMaximas", oEvento.PlazasMaximas - Miembros));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@PlazasActuales", oEvento.PlazasActuales - Miembros));
                        cmd.Parameters.Add(new SqlParameter("@PlazasMaximas", oEvento.PlazasMaximas + Miembros));
                    }
                    
                    cmd.Parameters.Add(new SqlParameter("@IdEvento", oEvento.IdEvento));
                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FechasEvento>> GetFechasEvento(DateTime dFechaInicio, DateTime dFechaFin)
        {
            List<FechasEvento> lstFechasEvento = new List<FechasEvento>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetComboFechas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", dFechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFin", dFechaFin));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        int nContador = 1;
                        while (await dr.ReadAsync())
                        {
                            FechasEvento oFechaEvento = new FechasEvento();
                            oFechaEvento.IdListaFecha = nContador;
                            oFechaEvento.ItemFecha = (DateTime)dr["FechaEvento"];
                            oFechaEvento.ItemFechaFormateada = oFechaEvento.ItemFecha.ToShortDateString();
                            lstFechasEvento.Add(oFechaEvento);
                            nContador++;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstFechasEvento;
        }

        public List<MiembrosEvento> GetMiembrosEvento(EventoPago oEventoPago)
        {
            List<MiembrosEvento> lstMiembros = new List<MiembrosEvento>();

            for (int i = 0; i < oEventoPago.Evento.PlazasMaximas; i++)
            {
                MiembrosEvento oMiembro = new MiembrosEvento();
                oMiembro.IdListaMiembro = i + 1;
                oMiembro.ItemMiembro = i + 1;

                lstMiembros.Add(oMiembro);
            }

            return lstMiembros;
        }
    }
}
