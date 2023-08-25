using System.Security.Cryptography.Xml;

namespace WhatWeDo.Recursos
{
    public class CargasCombo
    {
        public class FechasEvento
        {
            //Combo fechas de evento
            public int IdListaFecha { get; set; }

            public DateTime ItemFecha { get; set; }

            public string ItemFechaFormateada { get; set; }
        }

        public class MiembrosEvento
        {
            //Combo miembros

            public int IdListaMiembro { get; set; }

            public int ItemMiembro { get; set; }
        }
    }
}
