using System;

namespace CasoEstudio1DAL.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Identificacion { get; set; } 
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; } 
    }
}