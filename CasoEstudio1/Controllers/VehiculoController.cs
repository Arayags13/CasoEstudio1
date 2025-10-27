using CasoEstudio1BLL.Dtos;
using CasoEstudio1BLL.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasoEstudio1.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly IVehiculoServicio _vehiculoServicio;
        private readonly IClienteServicio _clienteServicio;

        public VehiculoController(IVehiculoServicio vehiculoServicio, IClienteServicio clienteServicio)
        {
            _vehiculoServicio = vehiculoServicio;
            _clienteServicio = clienteServicio;
        }
        public async Task<IActionResult> Index()
        {
            var clientesResponse = await _clienteServicio.ObtenerClientesAsync();
            ViewBag.Clientes = clientesResponse.Data ?? new List<ClienteDto>();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _vehiculoServicio.ObtenerVehiculosAsync();
            if (response.EsError)
            {
                return StatusCode(500, new { success = false, message = response.Mensaje });
            }
            return Json(new { data = response.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] VehiculoDto vehiculoDto)
        {
            CustomResponse<VehiculoDto> response;

            if (vehiculoDto.Id == 0)
            {
                response = await _vehiculoServicio.AgregarVehiculoAsync(vehiculoDto);
            }
            else
            {
                response = await _vehiculoServicio.ActualizarVehiculoAsync(vehiculoDto);
            }

            if (response.EsError)
            {
                return BadRequest(new { success = false, message = response.Mensaje });
            }

            return Ok(new { success = true, message = response.Mensaje });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _vehiculoServicio.EliminarVehiculoAsync(id);

            if (response.EsError)
            {
                return BadRequest(new { success = false, message = response.Mensaje });
            }

            return Ok(new { success = true, message = response.Mensaje });
        }
    }
}