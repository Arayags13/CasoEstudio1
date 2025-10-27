using CasoEstudio1DAL.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasoEstudio1DAL.Repositorios
{
    public class VehiculoRepositorio : IVehiculoRepositorio
    {
        private List<Vehiculo> vehiculos = new List<Vehiculo>()
        {
            new Vehiculo { Id = 1, Placa = "PZA456", Marca = "Audi", Modelo = "R8", ClienteId = 1 },
        };

        public async Task<bool> AgregarVehiculoAsync(Vehiculo vehiculo)
        {
            vehiculo.Id = vehiculos.Any() ? vehiculos.Max(v => v.Id) + 1 : 1;
            vehiculos.Add(vehiculo);
            return await Task.FromResult(true);
        }

        public async Task<bool> ActualizarVehiculoAsync(Vehiculo vehiculo)
        {
            var existente = vehiculos.FirstOrDefault(v => v.Id == vehiculo.Id);
            if (existente == null) return await Task.FromResult(false);

            existente.Placa = vehiculo.Placa;
            existente.Marca = vehiculo.Marca;
            existente.Modelo = vehiculo.Modelo;
            existente.ClienteId = vehiculo.ClienteId;
            return await Task.FromResult(true);
        }

        public async Task<bool> EliminarVehiculoAsync(int id)
        {
            var vehiculo = vehiculos.FirstOrDefault(v => v.Id == id);
            if (vehiculo != null)
            {
                vehiculos.Remove(vehiculo);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Vehiculo> ObtenerVehiculoPorIdAsync(int id)
        {
            return await Task.FromResult(vehiculos.FirstOrDefault(v => v.Id == id));
        }

        public async Task<Vehiculo> ObtenerVehiculoPorPlacaAsync(string placa)
        {
            return await Task.FromResult(vehiculos.FirstOrDefault(v => v.Placa.ToUpper() == placa.ToUpper()));
        }

        public async Task<List<Vehiculo>> ObtenerVehiculosAsync()
        {
            return await Task.FromResult(vehiculos);
        }
    }
}