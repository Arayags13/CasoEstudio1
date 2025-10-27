using CasoEstudio1BLL.Dtos;
using CasoEstudio1BLL.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CasoEstudio1.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteServicio _clienteServicio;

        public ClienteController(IClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _clienteServicio.ObtenerClientesAsync();
            if (response.EsError)
            {
                return StatusCode(500, response.Mensaje);
            }
            return Json(new { data = response.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] ClienteDto clienteDto)
        {
            CustomResponse<ClienteDto> response;

            if (clienteDto.Id == 0)
            {
                response = await _clienteServicio.AgregarClienteAsync(clienteDto);
            }
            else
            {
                response = await _clienteServicio.ActualizarClienteAsync(clienteDto);
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
            var response = await _clienteServicio.EliminarClienteAsync(id);

            if (response.EsError)
            {
                return BadRequest(new { success = false, message = response.Mensaje });
            }

            return Ok(new { success = true, message = response.Mensaje });
        }
    }
}