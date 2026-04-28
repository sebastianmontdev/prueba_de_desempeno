using Microsoft.EntityFrameworkCore;
using VetClinic.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregamos soporte para Controladores y Vistas
builder.Services.AddControllersWithViews();

// Configuración de la conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configuración del pipeline de la aplicación (cómo se procesan las peticiones)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Definimos la ruta por defecto (donde inicia la aplicación)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
