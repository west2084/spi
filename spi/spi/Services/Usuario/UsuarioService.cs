
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using spi.Data;
using spi.Models;
using spi.Services.Usuario;
namespace spi.Services.Usuario
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Usuario?> ValidarUsuarioAsync(string usuario, string contrasena)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == usuario && u.Password == contrasena );
        }

    }
}
