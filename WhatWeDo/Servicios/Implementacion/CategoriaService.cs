using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using WhatWeDo.Models;
using WhatWeDo.Servicios.Configuracion;
using WhatWeDo.Servicios.Contratos;

namespace WhatWeDo.Servicios.Implementacion
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ConfiguracionConexion _conexion;

        public CategoriaService(IOptions<ConfiguracionConexion> conexion)
        {
            _conexion = conexion.Value;
        }

        public async Task DeleteCategoria(Categoria oCategoria)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCategoria", oCategoria.IdCategoria));
                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Categoria>> GetCategorias()
        {
            List<Categoria> lstCategorias = new List<Categoria>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetCategorias", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Categoria oCategoria = new Categoria();
                            oCategoria.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                            oCategoria.Nombre = dr["Nombre"].ToString();
                            lstCategorias.Add(oCategoria);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return lstCategorias;
        }

        public async Task<Categoria> GetCategoria(Categoria oCategoria)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetCategoriaPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCategoria", oCategoria.IdCategoria));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oCategoria.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                            oCategoria.Nombre = dr["Nombre"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oCategoria;
        }

        public async Task InsertCategoria(Categoria oCategoria)
        {

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Nombre", oCategoria.Nombre));
                    
                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateCategoria(Categoria oCategoria)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCategoria", oCategoria.IdCategoria));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", oCategoria.Nombre));

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
