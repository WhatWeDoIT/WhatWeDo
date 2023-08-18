namespace WhatWeDo.Models
{
    public class Descuento
    {
        public int IdDescuento { get; set; }
        public int Valor { get; set; }

        public Descuento()
        {

        }

        public Descuento(int idDescuento,int valor)
        {
            IdDescuento = idDescuento;
            Valor = valor;
        }

    }
}
