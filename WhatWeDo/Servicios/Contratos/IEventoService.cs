using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IEventoService
    {
        Task<List<Evento>> GetEventos(int idUsuario);

        Task<Evento> GetEventoPorId(int idEvento);

        Task<List<Evento>> GetEventosPorCategoria(int nCategoria);

        Task<List<Evento>> GetEventosPorUsuario(int idUsuario);

        Task<List<Evento>> GetEventosPorUsuarioCategoria(int idUsuario, int nCategoria);

        Task<List<Evento>> GetEventosPorEmpresa(int idEmpresa);

        Task<List<Evento>> GetEventosPorEmpresaCategoria(int idEmpresa, int nCategoria);

        Task InsertEvento(Evento oEvento, int idEmpresa);

        Task UpdateEvento(Evento oEvento);

        Task DeleteEvento(Evento oEvento);

        Task<bool> GetReservado(int idUsuario, int idEvento);

        Task ReservarEvento(int idEvento, int idUsuario);

        Task AnularReservaEvento(int idEvento, int idUsuario);

        Task ModificarPlazasEvento(Evento oEvento, bool bReservar);
    }
}
