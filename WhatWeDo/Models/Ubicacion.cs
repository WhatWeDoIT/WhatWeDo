namespace WhatWeDo.Models
{
    public class Ubicacion
    {
        public int IdUbicacion { get; set; }
        public string Direccion { get; set; }

        public Ubicacion()
        {

        }

        public Ubicacion(int idUbicacion, string direccion)
        {
            this.IdUbicacion = idUbicacion;
            this.Direccion = direccion;
        }
    }
}
