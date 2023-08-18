using System.Data.SqlClient;
using System.Data;
using WhatWeDo.Models;
using WhatWeDo.Servicios.Contratos;
using Microsoft.Extensions.Options;
using WhatWeDo.Servicios.Configuracion;

namespace WhatWeDo.Servicios.Implementacion
{
    public class DescuentoService : IDescuentoService
    {
        private readonly ConfiguracionConexion _conexion;

        public DescuentoService(IOptions<ConfiguracionConexion> conexion)
        {
            _conexion = conexion.Value;
        }
        public async Task DeleteDescuento(Descuento oDescuento)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteDescuento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdDescuento", oDescuento.IdDescuento));
                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Descuento> GetDescuento(Descuento oDescuento)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetDescuentoPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdDescuento", oDescuento.IdDescuento));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oDescuento.IdDescuento = Convert.ToInt32(dr["IdDescuento"]);
                            oDescuento.Valor = int.Parse(dr["Valor"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oDescuento;
        }

        public async Task InsertDescuento(Descuento oDescuento)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertDescuento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Valor", oDescuento.Valor));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateDescuento(Descuento oDescuento)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdDescuento", oDescuento.IdDescuento));
                    cmd.Parameters.Add(new SqlParameter("@Valor", oDescuento.Valor));

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
