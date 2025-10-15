using Microsoft.EntityFrameworkCore;
using spi.Data;

namespace spi.Services.Evidencias
{
    public class EvidenciasService
    {
        private readonly ApplicationDbContext _context;

        public EvidenciasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<spi.Models.Evidencias>> GetAllEvidenciasAsync()
        {
            return await _context.Evidencias.Include(o => o.Observaciones).Include(o => o.Observaciones.Area).Include(o => o.Observaciones.Proyecto).ToListAsync();
        }



        public async Task<List<spi.Models.Evidencias>> GetObsEvidenciasAsync(int ObservacionesId)
        {
            return await _context.Evidencias.Include(o => o.Observaciones).Include(o => o.Observaciones.Area).Include(o => o.Observaciones.Proyecto).Where(o => o.ObservacionesId == ObservacionesId).ToListAsync();
        }


        public async Task<spi.Models.Observaciones> GetDatosEvidenciasAsync(int ObservacionesId)
        {
            return await _context.Observaciones.Include(o => o.Area).Include(o => o.Proyecto).FirstOrDefaultAsync(o=> o.Id==ObservacionesId);
        }


        public async Task AddEvidenciasAsync(spi.Models.Evidencias evidencias)
        {
            _context.Evidencias.Add(evidencias);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEvidenciasAsync(spi.Models.Evidencias evidencias)
        {
            _context.Entry(evidencias).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEvidenciasAsync(int id)
        {
            var evidencias = await _context.Evidencias.FindAsync(id);
            if (evidencias != null)
            {
                _context.Evidencias.Remove(evidencias);
                await _context.SaveChangesAsync();
            }
        }



    }
}
