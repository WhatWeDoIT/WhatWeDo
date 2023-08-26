using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IEmpresaService
    {
        Task<Empresa> GetEmpresa(Empresa oEmpresa);
        Task<Empresa> GetEmpresa(string mail);
        Task<Empresa> LoginEmpresa(string sEmail, string sPassword);

        Task<string> InsertEmpresa(Empresa oEmpresa);

        Task UpdateEmpresa(Empresa oEmpresa);

        Task DeleteEmpresa(Empresa oEmpresa);

        Task<Empresa> GetEmpresaPorIdEvento(int IdEvento);

    }
}
