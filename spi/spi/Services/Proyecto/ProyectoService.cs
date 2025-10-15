using Microsoft.EntityFrameworkCore;
using spi.Data;
using spi.Models;

namespace spi.Services.Proyecto
{
    public class ProyectoService
    {

        private readonly ApplicationDbContext _context;

        public ProyectoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<spi.Models.Proyecto>> GetAllProyectosAsync()
        {
            return await _context.Proyectos.ToListAsync();
        }

        public async Task<spi.Models.Proyecto> GetProyectoByIdAsync(int id)
        {
            return await _context.Proyectos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProyectoAsync(spi.Models.Proyecto proyecto)
        {
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProyectoAsync(spi.Models.Proyecto proyecto)
        {
            _context.Entry(proyecto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProyectoAsync(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto != null)
            {
                _context.Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();
            }
        }


    }
}
