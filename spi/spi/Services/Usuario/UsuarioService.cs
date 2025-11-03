
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

        public async Task<(Models.Usuario? user, string error)> ValidarUsuarioAsync(string username, string password)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

            if (usuario == null)
                return (null, "El usuario no existe.");

            if (usuario.Password != password)
                return (null, "La contraseña es incorrecta.");

            return (usuario, "");
        }


        // CRUD: Listar todos (incluye Area)
        public async Task<List<Models.Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Area)
                .ToListAsync();
        }

        // Obtener por id
        public async Task<Models.Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Area)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Crear
        public async Task<Models.Usuario> CreateAsync(Models.Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        // Actualizar
        public async Task<Models.Usuario> UpdateAsync(Models.Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        // Eliminar
        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        // Obtener usuario por Username
        public async Task<Models.Usuario?> GetByUsernameAsync(string username)
        {
            return await _context.Usuarios
                .Include(u => u.Area) // Incluimos el área para poder filtrar después
                .FirstOrDefaultAsync(u => u.Username == username);
        }

    }
}
