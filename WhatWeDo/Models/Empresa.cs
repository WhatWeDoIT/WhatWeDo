namespace WhatWeDo.Models
{
    public class Empresa
    {
        public int IdEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Pass { get; set; }
        public string Direccion { get; set; }
        public string Mail { get; set; }
        public double Saldo { get; set; }
        public string ConfirmPass { get; set; } //para validar la contraseña

        public Empresa()
        {
            
        }

        public Empresa(int idEmpresa, string nombre, string pass
                      ,string direccion, string mail)
        {
            IdEmpresa = idEmpresa;
            Nombre = nombre;    
            Pass = pass;    
            Direccion = direccion;  
            Mail = mail;
        }
    }
}
