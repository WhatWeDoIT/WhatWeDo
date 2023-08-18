

using System.Data.SqlTypes;

namespace WhatWeDo.Models
{
    public class Evento 
    {
        public int IdEvento { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public SqlDateTime FechaInicio { get; set; }

        public SqlDateTime FechaFin { get; set; }

        public int PlazasActuales { get; set; }

        public int PlazasMaximas { get; set; }

        public SqlMoney Precio { get; set; }

        public string Imagen { get; set; } //se guarda el path

        public int FKIdPunto { get; set; }

        public int FKIdCategoria { get; set; }

        public int FKIdDescuento { get; set; }

        public int ValorEnPuntos { get; set; }

    }
}
