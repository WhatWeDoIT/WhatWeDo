

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

        //para saber si el usuario lo ha reservado
        public bool Reservado { get; set; } 

        //Cargar combo Direcciones
        public List<Ubicacion> lstUbicaciones { get; set; }

        //Cargar combo Categorias
        public List<Categoria> lstCategorias { get; set; }

        //Cargar combo Descuentos
        public List<Descuento> lstDescuento { get; set; }
        
        //Cargar categoria en targeta
        public string CategoriaNombre { get; set; }

        //Cargar ubicacion en targeta
        public string UbicacionNombre { get; set; }
    }
}
