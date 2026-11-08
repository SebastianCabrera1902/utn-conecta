using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTNConecta.API.DTOs;
using UTNConecta.API.Services;
using System.Security.Claims;

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
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Apellido))
                return BadRequest("El apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("El email es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Contrasena))
                return BadRequest("La contraseña es obligatoria.");

            if (dto.AnioGraduacion < 1950 || dto.AnioGraduacion > DateTime.UtcNow.Year)
                return BadRequest("El año de graduación no es válido.");

            if (!await _service.CarreraExisteAsync(dto.IdCarrera))
                return BadRequest("La carrera no existe.");

            var graduado = await _service.RegistrarAsync(dto);
            if (graduado == null)
                return BadRequest("El email ya está registrado.");
            return Ok(graduado);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var resultado = await _service.LoginAsync(dto.Email, dto.Contrasena);
            if (resultado == null) return Unauthorized("Email o contraseña incorrectos");
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tipo != "graduado" || idToken != id.ToString())
                return Forbid();

            var graduado = await _service.ObtenerPorIdAsync(id);
            if (graduado == null) return NotFound();
            return Ok(graduado);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Actualizar(int id, [FromBody] GraduadoActualizarDto dto)
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (tipo != "graduado" || idToken != id.ToString())
                return Forbid();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Apellido))
                return BadRequest("El apellido es obligatorio.");

            if (dto.AnioGraduacion < 1950 || dto.AnioGraduacion > DateTime.UtcNow.Year)
                return BadRequest("El año de graduación no es válido.");

            if (!string.IsNullOrWhiteSpace(dto.CvUrl))
            {
                var cv = dto.CvUrl.Trim();
                var esValido = cv.Length <= 500
                    && Uri.TryCreate(cv, UriKind.Absolute, out var uri)
                    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

                if (!esValido)
                    return BadRequest("El enlace al CV no es válido. Tiene que empezar con http:// o https://");
            }

            var graduado = await _service.ActualizarAsync(id, dto);
            if (graduado == null) return NotFound();
            return Ok(graduado);
        }
    }
}