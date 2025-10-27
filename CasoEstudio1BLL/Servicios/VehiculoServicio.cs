using AutoMapper;
using CasoEstudio1BLL.Dtos;
using CasoEstudio1DAL.Entidades;
using CasoEstudio1DAL.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1BLL.Servicios
{
    public class VehiculoServicio : IVehiculoServicio
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio; 
        private readonly IMapper _mapper;

        public VehiculoServicio(IVehiculoRepositorio vehiculoRepositorio, IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<VehiculoDto>> AgregarVehiculoAsync(VehiculoDto vehiculoDto)
        {
            var respuesta = new CustomResponse<VehiculoDto>();

            if (await _vehiculoRepositorio.ObtenerVehiculoPorPlacaAsync(vehiculoDto.Placa) != null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = $"La placa {vehiculoDto.Placa} ya está registrada a otro vehículo.";
                return respuesta;
            }

            if (vehiculoDto.ClienteId == null || await _clienteRepositorio.ObtenerClientePorIdAsync(vehiculoDto.ClienteId.Value) == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Debe enlazar el vehículo a un cliente existente.";
                return respuesta;
            }

            var vehiculo = _mapper.Map<Vehiculo>(vehiculoDto);

            if (!await _vehiculoRepositorio.AgregarVehiculoAsync(vehiculo))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo registrar el vehículo.";
            }

            return respuesta;
        }

        public async Task<CustomResponse<VehiculoDto>> ActualizarVehiculoAsync(VehiculoDto vehiculoDto)
        {
            var respuesta = new CustomResponse<VehiculoDto>();

            var vehiculoPorPlaca = await _vehiculoRepositorio.ObtenerVehiculoPorPlacaAsync(vehiculoDto.Placa);
            if (vehiculoPorPlaca != null && vehiculoPorPlaca.Id != vehiculoDto.Id)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = $"La placa {vehiculoDto.Placa} ya pertenece a otro vehículo.";
                return respuesta;
            }

            if (!await _vehiculoRepositorio.ActualizarVehiculoAsync(_mapper.Map<Vehiculo>(vehiculoDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el vehículo";
            }
            return respuesta;
        }

        public async Task<CustomResponse<VehiculoDto>> EliminarVehiculoAsync(int id)
        {
            var respuesta = new CustomResponse<VehiculoDto>();

            if (!await _vehiculoRepositorio.EliminarVehiculoAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar el vehículo (posiblemente no existe)";
            }
            return respuesta;
        }

        public async Task<CustomResponse<VehiculoDto>> ObtenerVehiculoPorIdAsync(int id)
        {
            var respuesta = new CustomResponse<VehiculoDto>();
            var vehiculo = await _vehiculoRepositorio.ObtenerVehiculoPorIdAsync(id);

            if (vehiculo == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Vehículo no encontrado";
                return respuesta;
            }

            respuesta.Data = _mapper.Map<VehiculoDto>(vehiculo);
            return respuesta;
        }

        public async Task<CustomResponse<List<VehiculoDto>>> ObtenerVehiculosAsync()
        {
            var respuesta = new CustomResponse<List<VehiculoDto>>();
            var vehiculos = await _vehiculoRepositorio.ObtenerVehiculosAsync();
            respuesta.Data = _mapper.Map<List<VehiculoDto>>(vehiculos);
            return respuesta;
        }
    }
}