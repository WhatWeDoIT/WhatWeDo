using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface ICategoriaService
    {
        Task<Categoria> GetCategoria(Categoria oCategoria);
        Task InsertCategoria(Categoria oCategoria);

        Task UpdateCategoria(Categoria oCategoria);

        Task DeleteCategoria(Categoria oCategoria);
    }
}
