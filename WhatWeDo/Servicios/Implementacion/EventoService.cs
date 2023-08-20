using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using WhatWeDo.Servicios.Configuracion;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Models;
using System.Data.SqlTypes;

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

        public async Task<List<Evento>> GetEventos ()
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
        
        public async Task InsertEvento(Evento oEvento)
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

       
    }
}
