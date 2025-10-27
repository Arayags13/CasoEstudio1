using CasoEstudio1DAL.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasoEstudio1DAL.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private List<Cliente> clientes = new List<Cliente>()
        {
            new Cliente { Id = 1, Identificacion = "118250680", Nombre = "Sebastián", Apellido = "Araya", Edad = 24 },          
        };

        public async Task<bool> AgregarClienteAsync(Cliente cliente)
        {
            cliente.Id = clientes.Any() ? clientes.Max(u => u.Id) + 1 : 1;
            clientes.Add(cliente);
            return await Task.FromResult(true);
        }

        public async Task<bool> ActualizarClienteAsync(Cliente cliente)
        {
            var existente = clientes.FirstOrDefault(c => c.Id == cliente.Id);
            if (existente == null) return await Task.FromResult(false);

            existente.Nombre = cliente.Nombre;
            existente.Apellido = cliente.Apellido;
            existente.Edad = cliente.Edad;
            existente.Identificacion = cliente.Identificacion;

            return await Task.FromResult(true);
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            var cliente = clientes.FirstOrDefault(u => u.Id == id);
            if (cliente != null)
            {
                clientes.Remove(cliente);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Cliente> ObtenerClientePorIdAsync(int id)
        {
            return await Task.FromResult(clientes.FirstOrDefault(u => u.Id == id));
        }

        public async Task<Cliente> ObtenerClientePorIdentificacionAsync(string identificacion)
        {
            return await Task.FromResult(clientes.FirstOrDefault(u => u.Identificacion == identificacion));
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            return await Task.FromResult(clientes);
        }
    }
}