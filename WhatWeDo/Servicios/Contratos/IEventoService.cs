using WhatWeDo.Models;
using static WhatWeDo.Recursos.CargasCombo;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IEventoService
    {        
        Task<Evento> GetEventoPorId(int idEvento);

        Task<EventoPago> GetEventoPago(int idEvento, int idUsuario);

        Task<List<Evento>> GetEventos(int idUsuario);

        Task<List<Evento>> GetEventosPorCategoria(int nCategoria);

        Task<List<Evento>> GetEventosPorUsuario(int idUsuario);

        Task<List<Evento>> GetEventosPorUsuarioCategoria(int idUsuario, int nCategoria);

        Task<List<Evento>> GetEventosPorEmpresa(int idEmpresa);

        Task<List<Evento>> GetEventosPorEmpresaCategoria(int idEmpresa, int nCategoria);
        
        Task InsertEvento(Evento oEvento, int idEmpresa);

        Task InsertEventoPago(EventoPago oEventoPago);

        Task UpdateEvento(Evento oEvento);

        Task DeleteEvento(Evento oEvento);

        Task<bool> GetReservado(int idUsuario, int idEvento);

        Task ReservarEvento(int idEvento, int idUsuario, DateTime fecha, int miembros);

        Task AnularReservaEvento(int idEvento, int idUsuario, int miembros);

        Task ModificarPlazasEvento(Evento oEvento, bool bReservar, int Miembros);

        Task<List<FechasEvento>> GetFechasEvento(DateTime dFechaInicio, DateTime dFechaFin);

        List<MiembrosEvento> GetMiembrosEvento(EventoPago oEventoPago);
    }
}
