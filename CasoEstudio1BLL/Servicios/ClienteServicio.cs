using AutoMapper;
using CasoEstudio1BLL.Dtos;
using CasoEstudio1DAL.Entidades;
using CasoEstudio1DAL.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1BLL.Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        private const int MIN_AGE = 18;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IMapper _mapper;

        public ClienteServicio(IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            _clienteRepositorio = clienteRepositorio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<ClienteDto>> AgregarClienteAsync(ClienteDto clienteDto)
        {
            var respuesta = new CustomResponse<ClienteDto>();

            if (clienteDto.Edad < MIN_AGE)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No puede registrar clientes menores de edad.";
                return respuesta;
            }

            if (await _clienteRepositorio.ObtenerClientePorIdentificacionAsync(clienteDto.Identificacion) != null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = $"Ya existe un cliente con la identificación {clienteDto.Identificacion}.";
                return respuesta;
            }

            var cliente = _mapper.Map<Cliente>(clienteDto);

            if (!await _clienteRepositorio.AgregarClienteAsync(cliente))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo agregar el cliente al repositorio.";
            }

            return respuesta;
        }

        public async Task<CustomResponse<ClienteDto>> ActualizarClienteAsync(ClienteDto clienteDto)
        {
            var respuesta = new CustomResponse<ClienteDto>();

            if (clienteDto.Edad < MIN_AGE)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se puede actualizar el cliente, debe ser mayor de edad.";
                return respuesta;
            }

            var clienteAActualizar = await _clienteRepositorio.ObtenerClientePorIdAsync(clienteDto.Id);

            if (clienteAActualizar == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Cliente no encontrado para actualizar.";
                return respuesta;
            }

            var clientePorIdentificacion = await _clienteRepositorio.ObtenerClientePorIdentificacionAsync(clienteDto.Identificacion);

            if (clientePorIdentificacion != null && clientePorIdentificacion.Id != clienteDto.Id)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = $"Ya existe otro cliente con la identificación {clienteDto.Identificacion}.";
                return respuesta;
            }

            if (!await _clienteRepositorio.ActualizarClienteAsync(_mapper.Map<Cliente>(clienteDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el cliente";
            }
            return respuesta;
        }

        public async Task<CustomResponse<ClienteDto>> EliminarClienteAsync(int id)
        {
            var respuesta = new CustomResponse<ClienteDto>();

            if (!await _clienteRepositorio.EliminarClienteAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar el cliente (posiblemente no existe)";
            }
            return respuesta;
        }

        public async Task<CustomResponse<ClienteDto>> ObtenerClientePorIdAsync(int id)
        {
            var respuesta = new CustomResponse<ClienteDto>();
            var cliente = await _clienteRepositorio.ObtenerClientePorIdAsync(id);

            if (cliente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Cliente no encontrado";
                return respuesta;
            }

            respuesta.Data = _mapper.Map<ClienteDto>(cliente);
            return respuesta;
        }

        public async Task<CustomResponse<List<ClienteDto>>> ObtenerClientesAsync()
        {
            var respuesta = new CustomResponse<List<ClienteDto>>();
            var clientes = await _clienteRepositorio.ObtenerClientesAsync();
            respuesta.Data = _mapper.Map<List<ClienteDto>>(clientes);
            return respuesta;
        }
    }
}