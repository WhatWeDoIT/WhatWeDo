using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using WhatWeDo.Servicios.Configuracion;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ConfiguracionConexion _conexion;

        public UsuarioService(IOptions<ConfiguracionConexion> conexion)
        {
            _conexion = conexion.Value;
        }

        public async Task<Usuario> GetUsuario(Usuario oUsuario)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUsuarioPorMail", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mail", oUsuario.Mail));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oUsuario.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            oUsuario.Nombre = dr["Nombre"].ToString();
                            oUsuario.Pass = dr["Pass"].ToString();
                            oUsuario.Mail = dr["Mail"].ToString();
                            oUsuario.Miembros = Convert.ToInt32(dr["Miembros"]);
                            oUsuario.PuntosUsuario = Convert.ToInt32(dr["PuntosUsuario"]);
                            oUsuario.Saldo = Convert.ToDouble(dr["Saldo"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oUsuario;
        }

        public async Task<Usuario> GetUsuario(string mail)
        {
            Usuario oUsuario = new Usuario();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUsuarioPorMail", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mail", mail));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oUsuario.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            oUsuario.Nombre = dr["Nombre"].ToString();
                            oUsuario.Pass = dr["Pass"].ToString();
                            oUsuario.Mail = dr["Mail"].ToString();
                            oUsuario.Miembros = Convert.ToInt32(dr["Miembros"]);
                            oUsuario.PuntosUsuario = Convert.ToInt32(dr["PuntosUsuario"]);
                            oUsuario.Saldo = Convert.ToDouble(dr["Saldo"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oUsuario;
        }

        public async Task<Usuario> LoginUsuario(string sMail, string sPass)
        {
            Usuario oUsuario = new Usuario();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_LoginUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mail", sMail));
                    cmd.Parameters.Add(new SqlParameter("@Pass", sPass));

                    conexion.Open();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oUsuario.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            oUsuario.Nombre = dr["Nombre"].ToString();
                            oUsuario.Pass = dr["Pass"].ToString();
                            oUsuario.Mail = dr["Mail"].ToString();
                            oUsuario.Miembros = Convert.ToInt32(dr["Miembros"]);
                            oUsuario.PuntosUsuario = Convert.ToInt32(dr["PuntosUsuario"]);
                            oUsuario.Saldo = Convert.ToDouble(dr["Saldo"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
                       
            return oUsuario;
        }

        public async Task<string> InsertUsuario(Usuario oUsuario)
        {
            string sTransaccion = null;

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Nombre", oUsuario.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Pass", oUsuario.Pass));
                    cmd.Parameters.Add(new SqlParameter("@Mail", oUsuario.Mail));
                    cmd.Parameters.Add(new SqlParameter("@Miembros", oUsuario.Miembros));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            sTransaccion = dr["Transaccion"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return sTransaccion;
        }

        public async Task UpdateUsuario(Usuario oUsuario)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", oUsuario.IdUsuario));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", oUsuario.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Pass", oUsuario.Pass));
                    cmd.Parameters.Add(new SqlParameter("@Mail", oUsuario.Mail));
                    cmd.Parameters.Add(new SqlParameter("@Miembros", oUsuario.Miembros));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteUsuario(Usuario oUsuario)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", oUsuario.IdUsuario));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task PagarEvento(int idUsuario, int idEmpresa, double precio, int puntos)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_PagarEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpresa", idEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@Precio", precio));
                    cmd.Parameters.Add(new SqlParameter("@Puntos", puntos));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DevolucionEventoPago(int idUsuario, int idEmpresa, double precio, int puntos)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DevulocionPagoEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpresa", idEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@Precio", precio));
                    cmd.Parameters.Add(new SqlParameter("@Puntos", puntos));

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
