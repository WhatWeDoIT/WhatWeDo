using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string sEmail, string sPassword);
        Task<string> CrearUsuario(Usuario oUsuario);

    }
}
