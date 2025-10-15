using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace spi
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            // Aquí puedes validar si el usuario sigue existiendo en la base de datos, etc.
            // Si necesitas invalidar:
            // await _httpContextAccessor.HttpContext.SignOutAsync();

            return Task.FromResult(new AuthenticationState(user ?? new ClaimsPrincipal()));
        }
    }

}
