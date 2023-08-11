using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IUsuarioService
    {
        Task<Usuario> LoginUsuario(string sEmail, string sPassword);

        Task<string> InsertUsuario(Usuario oUsuario);

        void UpdateUsuario(Usuario oUsuario);

        void DeleteUsuario(Usuario oUsuario);

    }
}
