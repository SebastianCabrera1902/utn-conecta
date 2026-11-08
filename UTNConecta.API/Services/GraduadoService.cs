using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UTNConecta.API.Data;
using UTNConecta.API.DTOs;
using UTNConecta.API.Models;

namespace UTNConecta.API.Services
{
    public class GraduadoService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public GraduadoService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public async Task<GraduadoDto?> RegistrarAsync(GraduadoRegistroDto dto)
        {
            var existe = await _context.Graduados.AnyAsync(g => g.Email == dto.Email);
            if (existe) return null;

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
                AnioGraduacion = graduado.AnioGraduacion,
                CvUrl = graduado.CvUrl
            };
        }


        public async Task<bool> CarreraExisteAsync(int idCarrera)
        {
            return await _context.Carreras.AnyAsync(c => c.Id == idCarrera);
        }


        public async Task<LoginResponseDto?> LoginAsync(string email, string contrasena)
        {
            var graduado = await _context.Graduados
                .FirstOrDefaultAsync(g => g.Email == email);

            if (graduado == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(contrasena, graduado.Contrasena)) return null;

            var token = GenerarToken(graduado);

            return new LoginResponseDto
            {
                Token = token,
                Tipo = "graduado",
                Graduado = new GraduadoDto
                {
                    Id = graduado.Id,
                    Nombre = graduado.Nombre,
                    Apellido = graduado.Apellido,
                    Email = graduado.Email,
                    IdCarrera = graduado.IdCarrera,
                    AnioGraduacion = graduado.AnioGraduacion,
                    CvUrl = graduado.CvUrl
                }
            };
        }

        private string GenerarToken(Graduado graduado)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, graduado.Id.ToString()),
                new Claim(ClaimTypes.Email, graduado.Email),
                new Claim("nombre", graduado.Nombre),
                new Claim("tipo", "graduado")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<GraduadoDto?> ObtenerPorIdAsync(int id)
        {
            var graduado = await _context.Graduados.FindAsync(id);
            if (graduado == null) return null;

            return new GraduadoDto
            {
                Id = graduado.Id,
                Nombre = graduado.Nombre,
                Apellido = graduado.Apellido,
                Email = graduado.Email,
                IdCarrera = graduado.IdCarrera,
                AnioGraduacion = graduado.AnioGraduacion,
                CvUrl = graduado.CvUrl
            };
        }

        public async Task<GraduadoDto?> ActualizarAsync(int id, GraduadoActualizarDto dto)
        {
            var graduado = await _context.Graduados.FindAsync(id);
            if (graduado == null) return null;

            graduado.Nombre = dto.Nombre;
            graduado.Apellido = dto.Apellido;
            graduado.IdCarrera = dto.IdCarrera;
            graduado.AnioGraduacion = dto.AnioGraduacion;
            graduado.CvUrl = string.IsNullOrWhiteSpace(dto.CvUrl) ? null : dto.CvUrl.Trim();

            await _context.SaveChangesAsync();

            return new GraduadoDto
            {
                Id = graduado.Id,
                Nombre = graduado.Nombre,
                Apellido = graduado.Apellido,
                Email = graduado.Email,
                IdCarrera = graduado.IdCarrera,
                AnioGraduacion = graduado.AnioGraduacion,
                CvUrl = graduado.CvUrl
            };
        }
    }
}