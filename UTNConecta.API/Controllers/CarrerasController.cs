using Microsoft.AspNetCore.Mvc;
using UTNConecta.API.DTOs;
using UTNConecta.API.Services;

namespace UTNConecta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrerasController : ControllerBase
    {
        private readonly CarreraService _service;

        public CarrerasController(CarreraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carreras = await _service.GetAllAsync();
            return Ok(carreras);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id) { 
            var carreras = await _service.GetByIdAsync(id);
            if (carreras == null) return NotFound();
            return Ok(carreras);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarreraDto dto) {

            var creada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByID), new { id = creada.Id }, creada);

        }

    }
}