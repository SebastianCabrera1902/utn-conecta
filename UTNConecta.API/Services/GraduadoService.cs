using Microsoft.EntityFrameworkCore;
using UTNConecta.API.Data;
using UTNConecta.API.DTOs;
using UTNConecta.API.Models;

namespace UTNConecta.API.Services
{
    public class GraduadoService
    {
        private readonly AppDbContext _context;

        public GraduadoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GraduadoDto> RegistrarAsync(GraduadoRegistroDto dto)
        {
            var graduado = new Graduado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                IdCarrera = dto.IdCarrera,
                AnioGraduacion = dto.AnioGraduacion
            };

            _context.Graduados.Add(graduado);
            await _context.SaveChangesAsync();

            return new GraduadoDto
            {
                Id = graduado.Id,
                Nombre = graduado.Nombre,
                Apellido = graduado.Apellido,
                Email = graduado.Email,
                IdCarrera = graduado.IdCarrera,
                AnioGraduacion = graduado.AnioGraduacion
            };
        }

        public async Task<GraduadoDto?> LoginAsync(string email, string contrasena)
        {
            var graduado = await _context.Graduados
                .FirstOrDefaultAsync(g => g.Email == email);

            if (graduado == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(contrasena, graduado.Contrasena)) return null;

            return new GraduadoDto
            {
                Id = graduado.Id,
                Nombre = graduado.Nombre,
                Apellido = graduado.Apellido,
                Email = graduado.Email,
                IdCarrera = graduado.IdCarrera,
                AnioGraduacion = graduado.AnioGraduacion
            };
        }
    }
}