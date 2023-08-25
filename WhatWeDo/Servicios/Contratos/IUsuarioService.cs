using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string mail);
        Task<Usuario> GetUsuario(Usuario oUsuario);

        Task<Usuario> LoginUsuario(string sEmail, string sPassword);

        Task<string> InsertUsuario(Usuario oUsuario);

        Task UpdateUsuario(Usuario oUsuario);

        Task DeleteUsuario(Usuario oUsuario);

        Task PagarEvento(int idUsuario, int idEmpresa, double precio, int puntos);

        Task DevolucionEventoPago(int idUsuario, int idEmpresa, double precio, int puntos);
    }
}
