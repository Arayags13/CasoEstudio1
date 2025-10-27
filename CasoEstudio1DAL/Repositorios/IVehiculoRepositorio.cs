using CasoEstudio1DAL.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1DAL.Repositorios
{
    public interface IVehiculoRepositorio
    {
        Task<List<Vehiculo>> ObtenerVehiculosAsync();
        Task<Vehiculo> ObtenerVehiculoPorIdAsync(int id);
        Task<Vehiculo> ObtenerVehiculoPorPlacaAsync(string placa);
        Task<bool> AgregarVehiculoAsync(Vehiculo vehiculo);
        Task<bool> ActualizarVehiculoAsync(Vehiculo vehiculo);
        Task<bool> EliminarVehiculoAsync(int id);
    }
}