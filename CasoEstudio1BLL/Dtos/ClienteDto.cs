using System.ComponentModel.DataAnnotations;

namespace CasoEstudio1BLL.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La edad es obligatoria.")]
        [Range(1, 150, ErrorMessage = "La edad debe ser un número válido.")]
        public int? Edad { get; set; }
    }
}