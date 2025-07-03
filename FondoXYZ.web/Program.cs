using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FondoXYZ.web.Data;
using FondoXYZ.web.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using FondoXYZ.web.Services.Interfaces;
using FondoXYZ.web.Services;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión de la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configuración de DbContext para usar SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuración para Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configuración de la vista y controladores
builder.Services.AddControllersWithViews();

// Configuración de correo electrónico
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Registro del servicio de disponibilidad
builder.Services.AddScoped<IDisponibilidadService, DisponibilidadService>();

// Registro del servicio de Reservas
builder.Services.AddScoped<IReservaService, ReservaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

