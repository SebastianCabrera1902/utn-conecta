using Microsoft.AspNetCore.Mvc;
using UTNConecta.API.DTOs;
using UTNConecta.API.Services;

namespace UTNConecta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabilidadesController : ControllerBase
    {
        private readonly HabilidadService _service;

        public HabilidadesController(HabilidadService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var habilidades = await _service.GetAllAsync();
            return Ok(habilidades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var habilidad = await _service.GetByIdAsync(id);
            if (habilidad == null) return NotFound();
            return Ok(habilidad);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HabilidadDto dto)
        {
            var creada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }
    }
}