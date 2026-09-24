using Microsoft.EntityFrameworkCore;
using UTNConecta.API.Data;
using UTNConecta.API.DTOs;
using UTNConecta.API.Models;

namespace UTNConecta.API.Services
{
    public class HabilidadService
    {
        private readonly AppDbContext _context;

        public HabilidadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HabilidadDto>> GetAllAsync()
        {
            return await _context.Habilidades
                .Select(h => new HabilidadDto
                {
                    Id = h.Id,
                    Nombre = h.Nombre,
                    Area = h.Area
                })
                .ToListAsync();
        }

        public async Task<HabilidadDto?> GetByIdAsync(int id)
        {
            var habilidad = await _context.Habilidades.FindAsync(id);
            if (habilidad == null) return null;

            return new HabilidadDto
            {
                Id = habilidad.Id,
                Nombre = habilidad.Nombre,
                Area = habilidad.Area
            };
        }

        public async Task<HabilidadDto> CreateAsync(HabilidadDto dto)
        {
            var habilidad = new Habilidad
            {
                Nombre = dto.Nombre,
                Area = dto.Area
            };

            _context.Habilidades.Add(habilidad);
            await _context.SaveChangesAsync();

            dto.Id = habilidad.Id;
            return dto;
        }
    }
}