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

        public async Task<Usuario> GetUsuario(string sEmail, string sPassword)
        {
            Usuario oUsuario = new Usuario();

            using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Email", sEmail));
                cmd.Parameters.Add(new SqlParameter("@Pass", sPassword));

                conexion.Open();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        oUsuario.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                        oUsuario.NombreUsuario = dr["NombreUsuario"].ToString();                      
                        oUsuario.Email = dr["Email"].ToString();
                        oUsuario.Pass = dr["Pass"].ToString();
                        oUsuario.Rol = bool.Parse(dr["Rol"].ToString());
                    }
                }
            }
            return oUsuario;
        }

        public async Task<string> CrearUsuario(Usuario oUsuario)
        {
            string sTransaccion = null;

            using (SqlConnection conexion = new SqlConnection(_conexion.CadenaBBDD))
            {
                SqlCommand cmd = new SqlCommand("sp_CrearUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NombreUsuario", oUsuario.NombreUsuario));               
                cmd.Parameters.Add(new SqlParameter("@Email", oUsuario.Email));
                cmd.Parameters.Add(new SqlParameter("@Pass", oUsuario.Pass));
                cmd.Parameters.Add(new SqlParameter("@Rol", oUsuario.Rol));

                conexion.Open();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        sTransaccion = dr["Transaccion"].ToString();
                    }
                }
            }

            return sTransaccion;
        }

    }
}
