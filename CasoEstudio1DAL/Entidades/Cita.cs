using System;

namespace CasoEstudio1DAL.Entidades
{
    public class Cita
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public DateTime FechaCita { get; set; }
        public string Estado { get; set; }
    }
}