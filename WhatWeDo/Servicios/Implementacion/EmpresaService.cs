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
    public class EmpresaService : IEmpresaService
    {
        private readonly ConfiguracionConexion _conexion;

        public EmpresaService(IOptions<ConfiguracionConexion> conexion)
        {
            _conexion = conexion.Value;
        }

        public async Task<Empresa> GetEmpresa(Empresa oEmpresa)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEmpresaPorMail", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mail", oEmpresa.Mail));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oEmpresa.IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
                            oEmpresa.Nombre = dr["Nombre"].ToString();
                            oEmpresa.Pass = dr["Pass"].ToString();
                            oEmpresa.Direccion = dr["Direccion"].ToString();
                            oEmpresa.Mail = dr["Mail"].ToString();
                            if (!Convert.IsDBNull(dr["Saldo"]))
                                oEmpresa.Saldo = Convert.ToDouble(dr["Saldo"]);
                            else
                                oEmpresa.Saldo = 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oEmpresa;
        }

        public async Task<Empresa> GetEmpresa(string mail)
        {
            Empresa oEmpresa = new Empresa();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEmpresaPorMail", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mail", mail));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oEmpresa.IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
                            oEmpresa.Nombre = dr["Nombre"].ToString();
                            oEmpresa.Pass = dr["Pass"].ToString();
                            oEmpresa.Direccion = dr["Direccion"].ToString();
                            oEmpresa.Mail = dr["Mail"].ToString();
                            if (!Convert.IsDBNull(dr["Saldo"]))
                                oEmpresa.Saldo = Convert.ToDouble(dr["Saldo"]);
                            else
                                oEmpresa.Saldo = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oEmpresa;
        }

        public async Task<Empresa> LoginEmpresa(string sMail, string sPass)
        {
            Empresa oEmpresa = new Empresa();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_LoginEmpresa", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mail", sMail));
                    cmd.Parameters.Add(new SqlParameter("@Pass", sPass));

                    conexion.Open();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            oEmpresa.IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
                            oEmpresa.Nombre = dr["Nombre"].ToString();
                            oEmpresa.Pass = dr["Pass"].ToString();
                            oEmpresa.Direccion = dr["Direccion"].ToString();
                            oEmpresa.Mail = dr["Mail"].ToString();
                            if (!Convert.IsDBNull(dr["Saldo"]))
                                oEmpresa.Saldo = Convert.ToDouble(dr["Saldo"]);
                            else
                                oEmpresa.Saldo = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
                       
            return oEmpresa;
        }

        public async Task<string> InsertEmpresa(Empresa oEmpresa)
        {
            string sTransaccion = null;

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertEmpresa", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Nombre", oEmpresa.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Pass", oEmpresa.Pass));
                    cmd.Parameters.Add(new SqlParameter("@Direccion", oEmpresa.Direccion));
                    cmd.Parameters.Add(new SqlParameter("@Mail", oEmpresa.Mail));

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

        public async Task UpdateEmpresa(Empresa oEmpresa)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateEmpresa", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpresa", oEmpresa.IdEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", oEmpresa.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Pass", oEmpresa.Pass));
                    cmd.Parameters.Add(new SqlParameter("@Direccion", oEmpresa.Direccion));
                    cmd.Parameters.Add(new SqlParameter("@Mail", oEmpresa.Mail));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteEmpresa(Empresa oEmpresa)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteEmpresa", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", oEmpresa.IdEmpresa));

                    conexion.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Empresa> GetEmpresaPorIdEvento(int IdEvento)
        {
            Empresa oEmpresa = new Empresa();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEmpresaPorIdEvento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEvento", IdEvento));

                    conexion.Open();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            
                            oEmpresa.IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
                            oEmpresa.Nombre = dr["Nombre"].ToString();
                            oEmpresa.Pass = dr["Pass"].ToString();
                            oEmpresa.Direccion = dr["Direccion"].ToString();
                            oEmpresa.Mail = dr["Mail"].ToString();
                            if (!Convert.IsDBNull(dr["Saldo"]))
                                oEmpresa.Saldo = Convert.ToDouble(dr["Saldo"]);
                            else
                                oEmpresa.Saldo = 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return oEmpresa;
        }


    }
}
