using Microsoft.AspNetCore.Mvc;
using UTNConecta.API.DTOs;
using UTNConecta.API.Services;

namespace UTNConecta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GraduadosController : ControllerBase
    {
        private readonly GraduadoService _service;

        public GraduadosController(GraduadoService service)
        {
            _service = service;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(GraduadoRegistroDto dto)
        {
            var graduado = await _service.RegistrarAsync(dto);
            return Ok(graduado);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var graduado = await _service.LoginAsync(dto.Email, dto.Contrasena);
            if (graduado == null) return Unauthorized("Email o contraseña incorrectos");
            return Ok(graduado);
        }
    }
}