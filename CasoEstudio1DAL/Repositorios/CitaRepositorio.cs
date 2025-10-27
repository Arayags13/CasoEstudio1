using CasoEstudio1DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasoEstudio1DAL.Repositorios
{
    public class CitaRepositorio : ICitaRepositorio
    {
        
        public const string ESTADO_INGRESADA = "Ingresada";
        public const string ESTADO_CANCELADA = "Cancelada";
        public const string ESTADO_CONCLUIDA = "Concluida";

        private List<Cita> citas = new List<Cita>()
        {
            new Cita { Id = 1, ClienteId = 1, VehiculoId = 1, FechaCita = DateTime.Now.AddDays(1), Estado = ESTADO_INGRESADA },
        };

        public async Task<bool> AgregarCitaAsync(Cita cita)
        {
            cita.Id = citas.Any() ? citas.Max(c => c.Id) + 1 : 1;
            cita.Estado = ESTADO_INGRESADA; 
            citas.Add(cita);
            return await Task.FromResult(true);
        }

        public async Task<bool> ActualizarCitaAsync(Cita cita)
        {
            var existente = citas.FirstOrDefault(c => c.Id == cita.Id);
            if (existente == null) return await Task.FromResult(false);

            existente.ClienteId = cita.ClienteId;
            existente.VehiculoId = cita.VehiculoId;
            existente.FechaCita = cita.FechaCita;
            existente.Estado = cita.Estado;

            return await Task.FromResult(true);
        }

        public async Task<bool> EliminarCitaAsync(int id)
        {
            var cita = citas.FirstOrDefault(c => c.Id == id);
            if (cita != null)
            {
                citas.Remove(cita);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Cita> ObtenerCitaPorIdAsync(int id)
        {
            return await Task.FromResult(citas.FirstOrDefault(c => c.Id == id));
        }

        public async Task<List<Cita>> ObtenerCitasAsync()
        {
            return await Task.FromResult(citas);
        }
    }
}