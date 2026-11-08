using Microsoft.EntityFrameworkCore;
using UTNConecta.API.Data;
using UTNConecta.API.DTOs;
using UTNConecta.API.Models;

namespace UTNConecta.API.Services
{
    public class OportunidadService
    {
        private readonly AppDbContext _context;

        public OportunidadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OportunidadDto> CrearAsync(int organizacionId, OportunidadCrearDto dto)
        {
            var oportunidad = new Oportunidad
            {
                OrganizacionId = organizacionId,
                Puesto = dto.Puesto,
                Descripcion = dto.Descripcion,
                Requisitos = dto.Requisitos,
                AreaProfesional = dto.AreaProfesional,
                Tipo = dto.Tipo,
                Modalidad = dto.Modalidad,
                DatosContacto = dto.DatosContacto
            };

            _context.Oportunidades.Add(oportunidad);
            await _context.SaveChangesAsync();

            return MapearDto(oportunidad, "");
        }

        public async Task<List<OportunidadDto>> ListarAsync(string? tipo, string? area)
        {
            var query = _context.Oportunidades
                .Include(o => o.Organizacion)
                .Where(o => o.Estado == "activa");

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(o => o.Tipo == tipo);

            if (!string.IsNullOrEmpty(area))
                query = query.Where(o => o.AreaProfesional == area);

            var lista = await query.ToListAsync();

            return lista.Select(o => MapearDto(o, o.Organizacion?.Nombre ?? "")).ToList();
        }


        public async Task<List<OportunidadDto>> ListarPorOrganizacionAsync(int organizacionId)
        {
            var lista = await _context.Oportunidades
                .Include(o => o.Organizacion)
                .Where(o => o.OrganizacionId == organizacionId)
                .OrderByDescending(o => o.FechaPublicacion)
                .ToListAsync();

            return lista.Select(o => MapearDto(o, o.Organizacion?.Nombre ?? "")).ToList();
        }



        public async Task<OportunidadDto?> ObtenerPorIdAsync(int id)
        {
            var o = await _context.Oportunidades
                .Include(o => o.Organizacion)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (o == null) return null;

            return MapearDto(o, o.Organizacion?.Nombre ?? "");
        }

        public async Task<bool> EliminarAsync(int id, int organizacionId)
        {
            var o = await _context.Oportunidades.FindAsync(id);
            if (o == null || o.OrganizacionId != organizacionId) return false;

            _context.Oportunidades.Remove(o);
            await _context.SaveChangesAsync();
            return true;
        }

        private OportunidadDto MapearDto(Oportunidad o, string nombreOrg) => new OportunidadDto
        {
            Id = o.Id,
            Puesto = o.Puesto,
            Descripcion = o.Descripcion,
            Requisitos = o.Requisitos,
            AreaProfesional = o.AreaProfesional,
            Tipo = o.Tipo,
            Modalidad = o.Modalidad,
            DatosContacto = o.DatosContacto,
            FechaPublicacion = o.FechaPublicacion,
            Estado = o.Estado,
            NombreOrganizacion = nombreOrg
        };
    }
}