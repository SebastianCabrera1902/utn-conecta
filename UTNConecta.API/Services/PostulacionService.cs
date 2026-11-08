using Microsoft.EntityFrameworkCore;
using UTNConecta.API.Data;
using UTNConecta.API.DTOs;
using UTNConecta.API.Models;

namespace UTNConecta.API.Services
{
    public class PostulacionService
    {
        private readonly AppDbContext _context;

        public PostulacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PostulacionDto?> PostularAsync(int graduadoId, PostulacionCrearDto dto)
        {
            // Verificar que no exista ya una postulación
            var yaPostulado = await _context.Postulaciones
                .AnyAsync(p => p.GraduadoId == graduadoId && p.OportunidadId == dto.OportunidadId);

            if (yaPostulado) return null;

            var postulacion = new Postulacion
            {
                GraduadoId = graduadoId,
                OportunidadId = dto.OportunidadId
            };

            _context.Postulaciones.Add(postulacion);
            await _context.SaveChangesAsync();

            await _context.Entry(postulacion)
                .Reference(p => p.Graduado).LoadAsync();
            await _context.Entry(postulacion)
                .Reference(p => p.Oportunidad).LoadAsync();

            return MapearDto(postulacion);
        }

        public async Task<List<PostulacionDto>> ListarPorOportunidadAsync(int oportunidadId)
        {
            var lista = await _context.Postulaciones
                .Include(p => p.Graduado)
                .Include(p => p.Oportunidad)
                .Where(p => p.OportunidadId == oportunidadId)
                .ToListAsync();

            return lista.Select(MapearDto).ToList();
        }

        public async Task<List<PostulacionDto>> ListarPorGraduadoAsync(int graduadoId)
        {
            var lista = await _context.Postulaciones
                .Include(p => p.Graduado)
                .Include(p => p.Oportunidad)
                .Where(p => p.GraduadoId == graduadoId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return lista.Select(MapearDto).ToList();
        }

        private PostulacionDto MapearDto(Postulacion p) => new PostulacionDto
        {
            Id = p.Id,
            GraduadoId = p.GraduadoId,
            OportunidadId = p.OportunidadId,
            Fecha = p.Fecha,
            Estado = p.Estado,
            NombreGraduado = $"{p.Graduado?.Nombre} {p.Graduado?.Apellido}".Trim(),
            EmailGraduado = p.Graduado?.Email ?? "",
            CvUrlGraduado = p.Graduado?.CvUrl,
            PuestoOportunidad = p.Oportunidad?.Puesto ?? ""
        };

        public async Task<bool> OportunidadExisteAsync(int oportunidadId)
        {
            return await _context.Oportunidades.AnyAsync(o => o.Id == oportunidadId);
        }

        public async Task<bool> OportunidadEsDeOrganizacionAsync(int oportunidadId, int organizacionId)
        {
            return await _context.Oportunidades
                .AnyAsync(o => o.Id == oportunidadId && o.OrganizacionId == organizacionId);
        }

    }
} 