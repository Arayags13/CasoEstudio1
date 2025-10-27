using CasoEstudio1BLL.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1BLL.Servicios
{
    public interface ICitaServicio
    {
        Task<CustomResponse<CitaDto>> ObtenerCitaPorIdAsync(int id);
        Task<CustomResponse<List<CitaDto>>> ObtenerCitasAsync();
        Task<CustomResponse<CitaDto>> AgregarCitaAsync(CitaDto citaDto);
        Task<CustomResponse<CitaDto>> ActualizarCitaAsync(CitaDto citaDto);
        Task<CustomResponse<CitaDto>> ActualizarEstadoCitaAsync(int id, string nuevoEstado);
        Task<CustomResponse<CitaDto>> EliminarCitaAsync(int id);
    }
}