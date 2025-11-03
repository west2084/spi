using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using spi.Data;
using spi.Models;

namespace spi.Services.Observaciones
{
    public class ObservacionesService
    {
        private readonly ApplicationDbContext _context;

        public ObservacionesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<spi.Models.Observaciones>> GetAllObservacionesAsync()
        {
            return await _context.Observaciones
                .Include(o => o.Proyecto)
                .Include(o => o.ObservacionAreas)
                    .ThenInclude(oa => oa.Area)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<spi.Models.Observaciones>> GetProyObservacionesAsync(int ProyectoId, ClaimsPrincipal? user)
        {
            if (user is null)
                return new List<spi.Models.Observaciones>();

            var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var areaClaim = user.FindFirst("AreaId")?.Value;
            int.TryParse(areaClaim, out var areaId);

            // Delegado: ve todas las observaciones del proyecto
            if (string.Equals(role, "Delegado", StringComparison.OrdinalIgnoreCase))
            {
                return await _context.Observaciones
                    .Include(o => o.Proyecto)
                    .Include(o => o.ObservacionAreas).ThenInclude(oa => oa.Area)
                    .Where(o => o.ProyectoId == ProyectoId)
                    .AsNoTracking()
                    .ToListAsync();
            }

            // Usuario y Administrador: sólo observaciones del proyecto asociadas a su área
            if (string.Equals(role, "Usuario", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                if (areaId <= 0)
                    return new List<spi.Models.Observaciones>();

                return await _context.Observaciones
                    .Include(o => o.Proyecto)
                    .Include(o => o.ObservacionAreas).ThenInclude(oa => oa.Area)
                    .Where(o => o.ProyectoId == ProyectoId && o.ObservacionAreas.Any(oa => oa.AreaId == areaId))
                    .AsNoTracking()
                    .ToListAsync();
            }

            return new List<spi.Models.Observaciones>();
        }

        public async Task AddObservacionesAsync(spi.Models.Observaciones Observaciones)
        {
            _context.Observaciones.Add(Observaciones);
            await _context.SaveChangesAsync();
        }

        public async Task AddObservacionAreaAsync(ObservacionArea observacionArea)
        {
            _context.ObservacionAreas.Add(observacionArea);
            await _context.SaveChangesAsync();
        }

        public async Task<spi.Models.Observaciones?> GetObservacionesByIdAsync(int id)
        {
            return await _context.Observaciones
                .Include(o => o.Proyecto)
                .Include(o => o.ObservacionAreas)
                    .ThenInclude(oa => oa.Area)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateObservacionesAsync(spi.Models.Observaciones Observaciones)
        {
            _context.Entry(Observaciones).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteObservacionesAsync(int id)
        {
            var observacion = await _context.Observaciones
                .Include(o => o.ObservacionAreas)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (observacion != null)
            {
                _context.ObservacionAreas.RemoveRange(observacion.ObservacionAreas);
                _context.Observaciones.Remove(observacion);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<subconsulta>> GetGroupObsAsync(ClaimsPrincipal? user)
        {
            if (user is null)
                return new List<subconsulta>();

            var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var areaClaim = user.FindFirst("AreaId")?.Value;
            int.TryParse(areaClaim, out var areaId);

            var query = _context.ObservacionAreas
                .Include(oa => oa.Area)
                .Include(oa => oa.Observaciones)
                .AsQueryable();

            // Si no es Delegado, limitar a su área
            if (!string.Equals(role, "Delegado", StringComparison.OrdinalIgnoreCase))
            {
                if (areaId <= 0) return new List<subconsulta>();
                query = query.Where(oa => oa.AreaId == areaId);
            }

            var grouped = await query
                .GroupBy(oa => oa.Area.siglas_area)
                .Select(g => new
                {
                    Area = g.Key,
                    cantidad = g.Select(x => x.ObservacionesId).Distinct().Count()
                })
                .ToListAsync();

            return grouped.Select(g => new subconsulta
            {
                Area = g.Area,
                cantidad = g.cantidad
            }).ToList();
        }

        public List<subconsulta> GetGroupObs()
        {
            var query2 = (from x in _context.Observaciones.Include(x => x.Area).Include(x => x.Proyecto)
                          group new { x } by new { x.Area.siglas_area } into g
                          select new
                          {
                              Area = g.Key.siglas_area,
                              cantidad = g.Count(),
                          }).OrderByDescending(y => y.Area);

            List<subconsulta> list = new List<subconsulta>();
            foreach (var c in query2)
            {
                list.Add(new subconsulta
                {
                    Area = c.Area,
                    cantidad = c.cantidad,
                });
            }

            return list.ToList();
        }

        // Filtrado por rol y área: Usuario y Administrador solo ven las observaciones vinculadas a su área;
        // Delegado ve todas.
        public async Task<List<spi.Models.Observaciones>> GetObservacionesAsync(ClaimsPrincipal? user)
        {
            if (user is null)
                return new List<spi.Models.Observaciones>();

            var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var areaClaim = user.FindFirst("AreaId")?.Value;
            int.TryParse(areaClaim, out var areaId);

            // Delegado: ve todas las observaciones
            if (string.Equals(role, "Delegado", StringComparison.OrdinalIgnoreCase))
            {
                return await _context.Observaciones
                    .Include(o => o.Proyecto)
                    .Include(o => o.ObservacionAreas)
                        .ThenInclude(oa => oa.Area)
                    .AsNoTracking()
                    .ToListAsync();
            }

            // Usuario y Administrador: solo observaciones asociadas a su área (tabla puente)
            if (string.Equals(role, "Usuario", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                if (areaId <= 0)
                    return new List<spi.Models.Observaciones>();

                return await _context.Observaciones
                    .Include(o => o.Proyecto)
                    .Include(o => o.ObservacionAreas)
                        .ThenInclude(oa => oa.Area)
                    .Where(o => o.ObservacionAreas.Any(oa => oa.AreaId == areaId))
                    .AsNoTracking()
                    .ToListAsync();
            }

            // Roles no esperados: devolver vacío
            return new List<spi.Models.Observaciones>();
        }

        public async Task<List<spi.Models.Observaciones>> GetObservacionesPendientesAsync(ClaimsPrincipal? user)
        {
            if (user is null)
                return new List<spi.Models.Observaciones>();

            var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var areaClaim = user.FindFirst("AreaId")?.Value;
            int.TryParse(areaClaim, out var areaId);

            var baseQuery = _context.Observaciones
                .Include(o => o.ObservacionAreas)
                    .ThenInclude(oa => oa.Area)
                .Where(o => o.estatus == "Pendiente");

            // Delegado: ve todas las pendientes
            if (string.Equals(role, "Delegado", StringComparison.OrdinalIgnoreCase))
            {
                return await baseQuery
                    .Include(o => o.Proyecto)
                    .AsNoTracking()
                    .ToListAsync();
            }

            // Usuario y Administrador: sólo pendientes de su área
            if (string.Equals(role, "Usuario", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                if (areaId <= 0)
                    return new List<spi.Models.Observaciones>();

                baseQuery = baseQuery.Where(o => o.ObservacionAreas.Any(oa => oa.AreaId == areaId));

                return await baseQuery
                    .Include(o => o.Proyecto)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return new List<spi.Models.Observaciones>();
        }

        public async Task<List<spi.Models.Observaciones>> SearchObservacionesAsync(
    int proyectoId,
    ClaimsPrincipal? user,
    string searchTerm)
        {
            if (user is null)
                return new List<spi.Models.Observaciones>();

            var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var areaClaim = user.FindFirst("AreaId")?.Value;
            int.TryParse(areaClaim, out var areaId);

            // Base query: observaciones del proyecto
            var query = _context.Observaciones
                .Include(o => o.Proyecto)
                .Include(o => o.ObservacionAreas)
                    .ThenInclude(oa => oa.Area)
                .Where(o => o.ProyectoId == proyectoId)
                .AsQueryable();

            // Filtrado por rol
            if (string.Equals(role, "Usuario", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                if (areaId <= 0)
                    return new List<spi.Models.Observaciones>();

                query = query.Where(o => o.ObservacionAreas.Any(oa => oa.AreaId == areaId));
            }
            // Delegado ve todas, no se filtra por área

            // Filtrado por término de búsqueda
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(o =>
                    o.des_obs.ToLower().Contains(searchTerm) ||
                    o.indicador.ToLower().Contains(searchTerm) ||
                    o.estatus.ToLower().Contains(searchTerm) ||
                    o.Proyecto.des_proy.ToLower().Contains(searchTerm) ||   // 👈 nombre del proyecto
                    o.Proyecto.tipo_proy.ToLower().Contains(searchTerm) ||  // 👈 tipo de proyecto
                    o.ObservacionAreas.Any(oa => oa.Area.des_area.ToLower().Contains(searchTerm) ||
                                                 oa.Area.siglas_area.ToLower().Contains(searchTerm))
                );
            }

            return await query.AsNoTracking().ToListAsync();
        }

    }
}
