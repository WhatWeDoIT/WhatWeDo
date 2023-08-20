using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using WhatWeDo.Models;
using WhatWeDo.Servicios.Configuracion;
using WhatWeDo.Servicios.Contratos;

namespace WhatWeDo.Servicios.Implementacion
{
    public class UbicacionService : IUbicacionService
    {
        private readonly ConfiguracionConexion _conexion;

        public UbicacionService(IOptions<ConfiguracionConexion> conexion)
        {
            _conexion = conexion.Value;
        }
        public async Task DeleteUbicacion(Ubicacion oUbicacion)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteUbicacion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUbicacion", oUbicacion.IdUbicacion));
                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Ubicacion> GetUbicacion(Ubicacion oUbicacion)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUbicacionPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUbicacion", oUbicacion.IdUbicacion));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oUbicacion.IdUbicacion = Convert.ToInt32(dr["IdUbicacion"]);
                            oUbicacion.Direccion = dr["Direccion"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oUbicacion;
        }

        public async Task<List<Ubicacion>> GetUbicacionPorEmpresa(string sMail)
        {
            List<Ubicacion> lstUbicacion = new List<Ubicacion>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUbicacionPorEmpresa", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mail", sMail));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Ubicacion oUbicacion = new Ubicacion();
                            oUbicacion.IdUbicacion = Convert.ToInt32(dr["IdUbicacion"]);
                            oUbicacion.Direccion = dr["Direccion"].ToString();
                            oUbicacion.FkIdEmpresa = Convert.ToInt32(dr["FkIdEmpresa"]);

                            lstUbicacion.Add(oUbicacion);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstUbicacion;
        }

        public async Task InsertUbicacion(Ubicacion oUbicacion)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertUbicacion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Direccion", oUbicacion.Direccion));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUbicacion(Ubicacion oUbicacion)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUbicacion", oUbicacion.IdUbicacion));
                    cmd.Parameters.Add(new SqlParameter("@Direccion", oUbicacion.Direccion));

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
