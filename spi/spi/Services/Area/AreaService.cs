using Microsoft.EntityFrameworkCore;
using spi.Data;

namespace spi.Services.Area
{
    public class AreaService
    {


        private readonly ApplicationDbContext _context;

        public AreaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<spi.Models.Area>> GetAllAreaAsync()
        {
            return await _context.Areas.ToListAsync();
        }

        public async Task AddAreaAsync(spi.Models.Area area)
        {
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAreaAsync(spi.Models.Area area)
        {
            _context.Entry(area).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAreaAsync(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area != null)
            {
                _context.Areas.Remove(area);
                await _context.SaveChangesAsync();
            }
        }



    }
}
