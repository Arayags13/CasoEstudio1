using CasoEstudio1DAL.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1DAL.Repositorios
{
    public interface IClienteRepositorio
    {
        Task<List<Cliente>> ObtenerClientesAsync();
        Task<Cliente> ObtenerClientePorIdAsync(int id);
        Task<Cliente> ObtenerClientePorIdentificacionAsync(string identificacion);
        Task<bool> AgregarClienteAsync(Cliente cliente);
        Task<bool> ActualizarClienteAsync(Cliente cliente);
        Task<bool> EliminarClienteAsync(int id);
    }
}