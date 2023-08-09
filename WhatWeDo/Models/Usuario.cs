namespace WhatWeDo.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public bool Rol { get; set; }
    }
}
