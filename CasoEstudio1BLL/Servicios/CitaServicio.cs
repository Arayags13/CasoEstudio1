using AutoMapper;
using CasoEstudio1BLL.Dtos;
using CasoEstudio1DAL.Entidades;
using CasoEstudio1DAL.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasoEstudio1BLL.Servicios
{
    public class CitaServicio : ICitaServicio
    {
        private readonly ICitaRepositorio _citaRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly IMapper _mapper;

        public CitaServicio(ICitaRepositorio citaRepositorio, IClienteRepositorio clienteRepositorio, IVehiculoRepositorio vehiculoRepositorio, IMapper mapper)
        {
            _citaRepositorio = citaRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _vehiculoRepositorio = vehiculoRepositorio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<CitaDto>> AgregarCitaAsync(CitaDto citaDto)
        {
            var respuesta = new CustomResponse<CitaDto>();

            if (citaDto.ClienteId == null || await _clienteRepositorio.ObtenerClientePorIdAsync(citaDto.ClienteId.Value) == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "El cliente seleccionado no es válido.";
                return respuesta;
            }

            if (citaDto.VehiculoId == null || await _vehiculoRepositorio.ObtenerVehiculoPorIdAsync(citaDto.VehiculoId.Value) == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "El vehículo seleccionado no es válido.";
                return respuesta;
            }

            if (citaDto.FechaCita.HasValue && citaDto.FechaCita.Value < System.DateTime.Now)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La fecha de la cita debe ser futura.";
                return respuesta;
            }

            var cita = _mapper.Map<Cita>(citaDto);

            if (!await _citaRepositorio.AgregarCitaAsync(cita))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo registrar la cita.";
            }

            return respuesta;
        }

        public async Task<CustomResponse<CitaDto>> ActualizarCitaAsync(CitaDto citaDto)
        {

            var respuesta = new CustomResponse<CitaDto>();

            if (!await _citaRepositorio.ActualizarCitaAsync(_mapper.Map<Cita>(citaDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar la cita";
            }
            return respuesta;
        }

        public async Task<CustomResponse<CitaDto>> ActualizarEstadoCitaAsync(int id, string nuevoEstado)
        {
            var respuesta = new CustomResponse<CitaDto>();
            var cita = await _citaRepositorio.ObtenerCitaPorIdAsync(id);

            if (cita == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Cita no encontrada.";
                return respuesta;
            }

            var estadosValidos = new List<string> { CitaRepositorio.ESTADO_INGRESADA, CitaRepositorio.ESTADO_CANCELADA, CitaRepositorio.ESTADO_CONCLUIDA };
            if (!estadosValidos.Contains(nuevoEstado))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Estado de cita inválido.";
                return respuesta;
            }

            cita.Estado = nuevoEstado;

            if (!await _citaRepositorio.ActualizarCitaAsync(cita))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el estado de la cita.";
            }
            return respuesta;
        }

        public async Task<CustomResponse<CitaDto>> EliminarCitaAsync(int id)
        {
            var respuesta = new CustomResponse<CitaDto>();

            if (!await _citaRepositorio.EliminarCitaAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar la cita (posiblemente no existe)";
            }
            return respuesta;
        }

        public async Task<CustomResponse<CitaDto>> ObtenerCitaPorIdAsync(int id)
        {
            var respuesta = new CustomResponse<CitaDto>();
            var cita = await _citaRepositorio.ObtenerCitaPorIdAsync(id);

            if (cita == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Cita no encontrada";
                return respuesta;
            }

            respuesta.Data = _mapper.Map<CitaDto>(cita);
            return respuesta;
        }

        public async Task<CustomResponse<List<CitaDto>>> ObtenerCitasAsync()
        {
            var respuesta = new CustomResponse<List<CitaDto>>();
            var citas = await _citaRepositorio.ObtenerCitasAsync();
            var citasDto = new List<CitaDto>();

            foreach (var cita in citas)
            {
                var dto = _mapper.Map<CitaDto>(cita);
                var cliente = await _clienteRepositorio.ObtenerClientePorIdAsync(cita.ClienteId);
                var vehiculo = await _vehiculoRepositorio.ObtenerVehiculoPorIdAsync(cita.VehiculoId);

                dto.ClienteNombreCompleto = cliente != null ? $"{cliente.Nombre} {cliente.Apellido}" : "N/A";
                dto.VehiculoPlaca = vehiculo != null ? vehiculo.Placa : "N/A";
                citasDto.Add(dto);
            }

            respuesta.Data = citasDto;
            return respuesta;
        }
    }
}