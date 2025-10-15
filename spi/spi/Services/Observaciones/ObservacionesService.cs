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
            return await _context.Observaciones.Include(o => o.Proyecto).Include(o => o.ObservacionAreas)
                .ThenInclude(oa => oa.Area).ToListAsync();
        }

        public async Task<List<spi.Models.Observaciones>> GetProyObservacionesAsync(int ProyectoId)
        {
            return await _context.Observaciones.Include(o => o.Proyecto).Include(o => o.ObservacionAreas)
                .ThenInclude(oa => oa.Area).Where(o => o.ProyectoId == ProyectoId).ToListAsync();
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

        public async Task<spi.Models.Observaciones> GetObservacionesByIdAsync(int id)
        {
            return await _context.Observaciones.Include(o=>o.Area).Include(o=>o.Proyecto).Include(o => o.ObservacionAreas)
                .FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task UpdateObservacionesAsync(spi.Models.Observaciones Observaciones)
        {
            _context.Entry(Observaciones).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
        }

        public async Task DeleteObservacionesAsync(int id)
        {
            var observacion = await _context.Observaciones
        .Include(o => o.ObservacionAreas)
        .FirstOrDefaultAsync(o => o.Id == id);

            if (observacion != null)
            {
                // Eliminar primero las relaciones hijas
                _context.ObservacionAreas.RemoveRange(observacion.ObservacionAreas);

                // Luego eliminar la observación
                _context.Observaciones.Remove(observacion);

                await _context.SaveChangesAsync();
            }
        }


        public List<subconsulta> GetGroupObs()
        {

            var query2 = (from x in _context.Observaciones.Include(x => x.Area).Include(x=> x.Proyecto)
                          group new { x } by new { x.Area.siglas_area } into g
                          select
             new
             {
                 Area = g.Key.siglas_area,
                 cantidad = g.Count(),
             }
                         ).OrderByDescending(y => y.Area);

            List<subconsulta> list = new List<subconsulta>();
            foreach (var c in query2)
            {

                var asignar = new subconsulta
                {

                    Area = c.Area,
                    cantidad = c.cantidad,
                    
                };
                list.Add(asignar);

            }

            return list.ToList(); ;
        }


    }
}
