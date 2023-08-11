using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IEmpresaService
    {
        Task<Empresa> LoginEmpresa(string sEmail, string sPassword);

        Task<string> InsertEmpresa(Empresa oEmpresa);

        void UpdateEmpresa(Empresa oEmpresa);

        void DeleteEmpresa(Empresa oEmpresa);

    }
}
