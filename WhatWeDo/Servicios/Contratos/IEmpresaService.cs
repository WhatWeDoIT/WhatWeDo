using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IEmpresaService
    {
        Task<Empresa> LoginEmpresa(string sEmail, string sPassword);

        Task<string> InsertEmpresa(Empresa oEmpresa);

        Task UpdateEmpresa(Empresa oEmpresa);

        Task DeleteEmpresa(Empresa oEmpresa);

    }
}
