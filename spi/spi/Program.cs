using Microsoft.EntityFrameworkCore;
using Radzen;
using spi.Components;
using spi.Data;
using spi.Services.Proyecto;
using spi.Services.Area;
using spi.Services.Observaciones;
using spi.Services.Evidencias;
using spi.Services.File;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Error de conexion"));
});
builder.Services.AddRadzenComponents();

builder.Services.AddScoped<ProyectoService>();
builder.Services.AddScoped<AreaService>();
builder.Services.AddScoped<ObservacionesService>();
builder.Services.AddScoped<EvidenciasService>();
builder.Services.AddScoped<FileService>();



var app = builder.Build();

//


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

