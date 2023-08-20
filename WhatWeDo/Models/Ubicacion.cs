namespace WhatWeDo.Models
{
    public class Ubicacion
    {
        public int IdUbicacion { get; set; }
        public string Direccion { get; set; }
        
        public int FkIdEmpresa { get; set; }

        public Ubicacion()
        {

        }

        public Ubicacion(int idUbicacion, string direccion, int fkIdEmpresa)
        {
            this.IdUbicacion = idUbicacion;
            this.Direccion = direccion;
            this.FkIdEmpresa = fkIdEmpresa;
        }
    }
}
