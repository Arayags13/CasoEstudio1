using CasoEstudio1BLL.Mapeos;
using CasoEstudio1BLL.Servicios;
using CasoEstudio1DAL.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inyección de Dependencias (DI) de la Capa DAL
builder.Services.AddSingleton<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddSingleton<IVehiculoRepositorio, VehiculoRepositorio>();
builder.Services.AddSingleton<ICitaRepositorio, CitaRepositorio>();

// Inyección de Dependencias (DI) de la Capa BLL (Servicios)
builder.Services.AddSingleton<IClienteServicio, ClienteServicio>();
builder.Services.AddSingleton<IVehiculoServicio, VehiculoServicio>();
builder.Services.AddSingleton<ICitaServicio, CitaServicio>();

// Configuración de AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();