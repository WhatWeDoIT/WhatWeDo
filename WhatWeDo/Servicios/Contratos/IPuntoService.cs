using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IPuntoService
    {
            Task<Punto> GetUsuario(Punto oPunto);

            Task<Punto> LoginUsuario(string sEmail, string sPassword);

            Task<string> InsertPunto(Punto oPunto);

            Task UpdatePunto(Punto oPunto);

            Task DeletePunto(Punto oPunto);
    }
}
