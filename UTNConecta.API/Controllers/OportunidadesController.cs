using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTNConecta.API.DTOs;
using UTNConecta.API.Services;
using System.Security.Claims;

namespace UTNConecta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OportunidadesController : ControllerBase
    {
        private readonly OportunidadService _service;

        public OportunidadesController(OportunidadService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Listar([FromQuery] string? tipo, [FromQuery] string? area, [FromQuery] string? modalidad)
        {
            var lista = await _service.ListarAsync(tipo, area, modalidad);
            return Ok(lista);
        }

        [HttpGet("mias")]
        [Authorize]
        public async Task<IActionResult> ListarMias()
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tipo != "organizacion" || !int.TryParse(idClaim, out var organizacionId))
                return Forbid();

            var lista = await _service.ListarPorOrganizacionAsync(organizacionId);
            return Ok(lista);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var oportunidad = await _service.ObtenerPorIdAsync(id);
            if (oportunidad == null) return NotFound();
            return Ok(oportunidad);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear([FromBody] OportunidadCrearDto dto)
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tipo != "organizacion" || !int.TryParse(idClaim, out var organizacionId))
                return Forbid();

            if (string.IsNullOrWhiteSpace(dto.Puesto))
                return BadRequest("El puesto es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest("La descripción es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.Requisitos))
                return BadRequest("Los requisitos son obligatorios.");

            if (string.IsNullOrWhiteSpace(dto.AreaProfesional))
                return BadRequest("El área profesional es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.Modalidad))
                return BadRequest("La modalidad es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.DatosContacto))
                return BadRequest("Los datos de contacto son obligatorios.");

            var tiposValidos = new[] { "trabajo", "pasantía" };
            var tipoLimpio = dto.Tipo?.Trim().ToLowerInvariant();
            if (tipoLimpio == "pasantia") tipoLimpio = "pasantía";

            if (tipoLimpio == null || !tiposValidos.Contains(tipoLimpio))
                return BadRequest("El tipo tiene que ser trabajo o pasantía.");

            var contacto = dto.DatosContacto.Trim();
            var esEmail = contacto.Contains('@') && contacto.Contains('.');
            var cantidadDigitos = contacto.Count(char.IsDigit);
            if (!esEmail && cantidadDigitos < 6)
                return BadRequest("Los datos de contacto tienen que ser un email o un teléfono.");

            dto.Puesto = dto.Puesto.Trim();
            dto.Descripcion = dto.Descripcion.Trim();
            dto.Requisitos = dto.Requisitos.Trim();
            dto.AreaProfesional = dto.AreaProfesional.Trim();
            dto.Modalidad = dto.Modalidad.Trim();
            dto.DatosContacto = contacto;
            dto.Tipo = tipoLimpio;

            var oportunidad = await _service.CrearAsync(organizacionId, dto);
            return Ok(oportunidad);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tipo != "organizacion" || !int.TryParse(idClaim, out var organizacionId))
                return Forbid();

            var resultado = await _service.EliminarAsync(id, organizacionId);
            if (!resultado) return NotFound();
            return Ok("Oportunidad eliminada.");
        }

    }
}