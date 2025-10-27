using System;
using System.ComponentModel.DataAnnotations;

namespace CasoEstudio1BLL.Dtos
{
    public class CitaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo.")]
        public int? VehiculoId { get; set; }

        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        public DateTime? FechaCita { get; set; }

        public string Estado { get; set; } // Ingresada, Cancelada, Concluida

        public string ClienteNombreCompleto { get; set; }
        public string VehiculoPlaca { get; set; }
    }
}