using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IDescuentoService
    {
        Task<List<Descuento>> GetDescuentos();

        Task<Descuento> GetDescuento(Descuento oDescuento);

        Task InsertDescuento(Descuento oDescuento);

        Task UpdateDescuento(Descuento oDescuento);

        Task DeleteDescuento(Descuento oDescuento);
    }
}
