using CasoEstudio1BLL.Dtos;
using CasoEstudio1BLL.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1.Controllers
{
    public class CitaController : Controller
    {
        private readonly ICitaServicio _citaServicio;
        private readonly IClienteServicio _clienteServicio;
        private readonly IVehiculoServicio _vehiculoServicio;

        public CitaController(ICitaServicio citaServicio, IClienteServicio clienteServicio, IVehiculoServicio vehiculoServicio)
        {
            _citaServicio = citaServicio;
            _clienteServicio = clienteServicio;
            _vehiculoServicio = vehiculoServicio;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Clientes = (await _clienteServicio.ObtenerClientesAsync()).Data ?? new List<ClienteDto>();
            ViewBag.Vehiculos = (await _vehiculoServicio.ObtenerVehiculosAsync()).Data ?? new List<VehiculoDto>();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _citaServicio.ObtenerCitasAsync();
            if (response.EsError)
            {
                return StatusCode(500, new { success = false, message = response.Mensaje });
            }
            return Json(new { data = response.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] CitaDto citaDto)
        {
            CustomResponse<CitaDto> response;

            if (citaDto.Id == 0)
            {
                response = await _citaServicio.AgregarCitaAsync(citaDto);
            }
            else
            {
                response = await _citaServicio.ActualizarCitaAsync(citaDto);
            }

            if (response.EsError)
            {
                return BadRequest(new { success = false, message = response.Mensaje });
            }

            return Ok(new { success = true, message = response.Mensaje });
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, [FromQuery] string nuevoEstado)
        {
            var response = await _citaServicio.ActualizarEstadoCitaAsync(id, nuevoEstado);

            if (response.EsError)
            {
                return BadRequest(new { success = false, message = response.Mensaje });
            }

            return Ok(new { success = true, message = response.Mensaje });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _citaServicio.EliminarCitaAsync(id);

            if (response.EsError)
            {
                return BadRequest(new { success = false, message = response.Mensaje });
            }

            return Ok(new { success = true, message = response.Mensaje });
        }
    }
}