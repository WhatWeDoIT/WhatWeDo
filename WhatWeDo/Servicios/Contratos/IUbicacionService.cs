﻿using WhatWeDo.Models;

namespace WhatWeDo.Servicios.Contratos
{
    public interface IUbicacionService
    {
        Task<Ubicacion> GetUbicacion(Ubicacion oUbicacion);
        Task InsertUbicacion(Ubicacion oUbicacion);

        Task UpdateUbicacion(Ubicacion oUbicacion);

        Task DeleteUbicacion(Ubicacion oUbicacion);
    }
}
