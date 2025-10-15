namespace spi.Services.Usuario
{
    public interface IUsuarioService
    {
        
        

        public Task<Models.Usuario?> ValidarUsuarioAsync(string usuario, string contrasena);

    }
}
