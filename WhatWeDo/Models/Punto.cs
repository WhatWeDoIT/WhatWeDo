using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhatWeDo.Models
{
    public class Punto
    {
        public int IdPunto { get; set; }
        public string Valor { get; set; }

        public Punto()
        {

        }

        public Punto(int idPunto, string valor)
        {
            IdPunto = idPunto;
            Valor = valor;
        }
    }
}
