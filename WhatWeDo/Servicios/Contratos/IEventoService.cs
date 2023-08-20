using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IEventoService
    {
        Task<List<Evento>> GetEventos();

        Task<List<Evento>> GetEventosPorCategoria(int nCategoria);

        Task InsertEvento(Evento oEvento);

        Task UpdateEvento(Evento oEvento);

        Task DeleteEvento(Evento oEvento);

    }
}
