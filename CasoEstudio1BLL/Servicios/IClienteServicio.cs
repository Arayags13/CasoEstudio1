using CasoEstudio1BLL.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1BLL.Servicios
{
    public interface IClienteServicio
    {
        Task<CustomResponse<ClienteDto>> ObtenerClientePorIdAsync(int id);
        Task<CustomResponse<List<ClienteDto>>> ObtenerClientesAsync();
        Task<CustomResponse<ClienteDto>> AgregarClienteAsync(ClienteDto clienteDto);
        Task<CustomResponse<ClienteDto>> ActualizarClienteAsync(ClienteDto clienteDto);
        Task<CustomResponse<ClienteDto>> EliminarClienteAsync(int id);
    }
}