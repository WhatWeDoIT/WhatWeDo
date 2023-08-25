using Humanizer.DateTimeHumanizeStrategy;
using WhatWeDo.Recursos;
using static WhatWeDo.Recursos.CargasCombo;

namespace WhatWeDo.Models
{
    public class EventoPago
    {
        public int IdEventoPago { get; set; }
        public int FkIdEvento { get; set; }
        public int FkIdUsuario{ get; set; }
        public int FkIdEmpresa { get; set; }
        public DateTime FechaAsistencia { get; set; }
        public int Miembros { get; set; }
        public double PrecioTotal { get; set; }
        public int PuntosAsignados { get; set; }
       
        public Evento Evento { get; set; }  

        //para rellenar combo de fechas
        public List<FechasEvento> FechasEvento { get; set; }

        //para rellenar combo de miembros
        public List<MiembrosEvento> MiembrosEvento { get; set; }

        //Precio con simbolo
        public string PrecioConSimbolo { get; set; }

        public EventoPago()
        {

        }      
    }
}
