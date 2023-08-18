

using System.Data.SqlTypes;

namespace WhatWeDo.Models
{
    public class Evento 
    {
        public int IdEvento { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int PlazasActuales { get; set; }

        public int PlazasMaximas { get; set; }

        public double Precio { get; set; }

        public string Imagen { get; set; } //se guarda el path

        public int? FkIdPunto { get; set; }

        public int? FkIdUbicacion { get; set; }

        public int? FkIdCategoria { get; set; }

        public int? FkIdDescuento { get; set; }

        public int ValorEnPuntos { get; set; }

    }
}
