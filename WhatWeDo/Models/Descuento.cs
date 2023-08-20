namespace WhatWeDo.Models
{
    public class Descuento
    {
        public int IdDescuento { get; set; }
        public int Valor { get; set; }

        //Para mostrar el valor al usuario con el %
        public string MostrarValor { get; set; }

        public Descuento()
        {

        }

        public Descuento(int idDescuento,int valor, string mostrarValor)
        {
            IdDescuento = idDescuento;
            Valor = valor;
            this.MostrarValor = mostrarValor;
        }

    }
}
