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
    public class OrganizacionService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public OrganizacionService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<OrganizacionDto?> RegistrarAsync(OrganizacionRegistroDto dto)
        {
            var existe = await _context.Organizaciones.AnyAsync(o => o.Email == dto.Email);
            if (existe) return null;

            var organizacion = new Organizacion
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Tipo = dto.Tipo
            };

            _context.Organizaciones.Add(organizacion);
            await _context.SaveChangesAsync();

            return new OrganizacionDto
            {
                Id = organizacion.Id,
                Nombre = organizacion.Nombre,
                Email = organizacion.Email,
                Tipo = organizacion.Tipo
            };
        }



        public async Task<LoginResponseDto?> LoginAsync(string email, string contrasena)
        {
            var organizacion = await _context.Organizaciones
                .FirstOrDefaultAsync(o => o.Email == email);

            if (organizacion == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(contrasena, organizacion.Contrasena)) return null;

            var token = GenerarToken(organizacion);

            return new LoginResponseDto
            {
                Token = token,
                Tipo = "organizacion",
                Organizacion = new OrganizacionDto
                {
                    Id = organizacion.Id,
                    Nombre = organizacion.Nombre,
                    Email = organizacion.Email,
                    Tipo = organizacion.Tipo
                }
            };
        }

        private string GenerarToken(Organizacion organizacion)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, organizacion.Id.ToString()),
                new Claim(ClaimTypes.Email, organizacion.Email),
                new Claim("nombre", organizacion.Nombre),
                new Claim("tipo", "organizacion")
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
    }
}