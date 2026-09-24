using Microsoft.EntityFrameworkCore;
using UTNConecta.API.Data;
using UTNConecta.API.DTOs;
using UTNConecta.API.Models;

namespace UTNConecta.API.Services
{
    public class CarreraService
    {
        private readonly AppDbContext _context;

        public CarreraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarreraDto>> GetAllAsync()
        {
            return await _context.Carreras
                .Select(c => new CarreraDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Area = c.Area
                })
                .ToListAsync();
        }

        public async Task<CarreraDto?> GetByIdAsync(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null) return null;

            return new CarreraDto
            {
                Id = carrera.Id,
                Nombre = carrera.Nombre,
                Area = carrera.Area
            };
        }

        public async Task<CarreraDto> CreateAsync(CarreraDto dto)
        {
            var carrera = new Carrera
            {
                Nombre = dto.Nombre,
                Area = dto.Area
            };

            _context.Carreras.Add(carrera);
            await _context.SaveChangesAsync();

            dto.Id = carrera.Id;
            return dto;
        }
    }
}