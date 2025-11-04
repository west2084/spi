using Microsoft.EntityFrameworkCore;
using Radzen;
using spi.Components;
using spi.Data;
using spi.Services.Proyecto;
using spi.Services.Area;
using spi.Services.Observaciones;
using spi.Services.Evidencias;
using spi.Services.File;
using spi.Services.Usuario;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Error de conexion"));
});
builder.Services.AddRadzenComponents();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // redirige aquí si no está autenticado
        options.AccessDeniedPath = "/denegado";

        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddScoped<ProyectoService>();
builder.Services.AddScoped<AreaService>();
builder.Services.AddScoped<ObservacionesService>();
builder.Services.AddScoped<EvidenciasService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddHttpClient();

//autentificación
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromSeconds(3000);
    options.SlidingExpiration = false;
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


//app.MapBlazorHub();

app.MapGet("/login-handler", async context =>
{
    var username = context.Request.Query["user"];
    var role = context.Request.Query["role"];
    var areaId = context.Request.Query["areaId"];

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username!),
        new Claim(ClaimTypes.Role, role!),
        new Claim("AreaId", areaId!) // nuevo claim
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    context.Response.Redirect("/");
});


app.MapGet("/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/login");
});


app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();




app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

