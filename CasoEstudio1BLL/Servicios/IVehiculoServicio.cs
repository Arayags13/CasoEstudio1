using CasoEstudio1BLL.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1BLL.Servicios
{
    public interface IVehiculoServicio
    {
        Task<CustomResponse<VehiculoDto>> ObtenerVehiculoPorIdAsync(int id);
        Task<CustomResponse<List<VehiculoDto>>> ObtenerVehiculosAsync();
        Task<CustomResponse<VehiculoDto>> AgregarVehiculoAsync(VehiculoDto vehiculoDto);
        Task<CustomResponse<VehiculoDto>> ActualizarVehiculoAsync(VehiculoDto vehiculoDto);
        Task<CustomResponse<VehiculoDto>> EliminarVehiculoAsync(int id);
    }
}