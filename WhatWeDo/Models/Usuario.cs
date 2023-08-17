namespace WhatWeDo.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Pass { get; set; }
        public string Direccion { get; set; }
        public string Mail { get; set; }
        public int Miembros { get; set; }

        public string ConfirmPass { get; set; } //para validar la contraseña
        public bool EsEmpresa { get; set; } //para asignar rol en el login y registro
    }
}
