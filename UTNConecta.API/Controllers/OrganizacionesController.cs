using Microsoft.AspNetCore.Mvc;
using UTNConecta.API.DTOs;
using UTNConecta.API.Services;

namespace UTNConecta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizacionesController : ControllerBase
    {
        private readonly OrganizacionService _service;

        public OrganizacionesController(OrganizacionService service)
        {
            _service = service;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(OrganizacionRegistroDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("El email es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Contrasena))
                return BadRequest("La contraseña es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.Tipo))
                return BadRequest("El tipo de organización es obligatorio.");

            var organizacion = await _service.RegistrarAsync(dto);
            if (organizacion == null)
                return BadRequest("El email ya está registrado.");
            return Ok(organizacion);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var resultado = await _service.LoginAsync(dto.Email, dto.Contrasena);
            if (resultado == null) return Unauthorized("Email o contraseña incorrectos");
            return Ok(resultado);
        }
    }
}