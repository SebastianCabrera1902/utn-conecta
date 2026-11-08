using Microsoft.AspNetCore.Mvc;
using UTNConecta.API.DTOs;
using UTNConecta.API.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace UTNConecta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostulacionesController : ControllerBase
    {
        private readonly PostulacionService _service;

        public PostulacionesController(PostulacionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Postular([FromBody] PostulacionCrearDto dto)
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tipo != "graduado" || !int.TryParse(idClaim, out var graduadoId))
                return Forbid();

            if (!await _service.OportunidadExisteAsync(dto.OportunidadId))
                return NotFound("La oportunidad no existe.");

            var resultado = await _service.PostularAsync(graduadoId, dto);
            if (resultado == null)
                return BadRequest("Ya existe una postulación para esta oportunidad.");
            return Ok(resultado);
        }

        [HttpGet("mias")]
        [Authorize]
        public async Task<IActionResult> ListarMias()
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tipo != "graduado" || !int.TryParse(idClaim, out var graduadoId))
                return Forbid();

            var lista = await _service.ListarPorGraduadoAsync(graduadoId);
            return Ok(lista);
        }

        [HttpGet("oportunidad/{oportunidadId}")]
        [Authorize]
        public async Task<IActionResult> ListarPorOportunidad(int oportunidadId)
        {
            var tipo = User.FindFirst("tipo")?.Value;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tipo != "organizacion" || !int.TryParse(idClaim, out var organizacionId))
                return Forbid();

            if (!await _service.OportunidadExisteAsync(oportunidadId))
                return NotFound("La oportunidad no existe.");

            if (!await _service.OportunidadEsDeOrganizacionAsync(oportunidadId, organizacionId))
                return Forbid();

            var lista = await _service.ListarPorOportunidadAsync(oportunidadId);
            return Ok(lista);
        }

    }
}