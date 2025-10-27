using CasoEstudio1DAL.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1DAL.Repositorios
{
    public interface ICitaRepositorio
    {
        Task<List<Cita>> ObtenerCitasAsync();
        Task<Cita> ObtenerCitaPorIdAsync(int id);
        Task<bool> AgregarCitaAsync(Cita cita);
        Task<bool> ActualizarCitaAsync(Cita cita);
        Task<bool> EliminarCitaAsync(int id);
    }
}