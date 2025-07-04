using FondoXYZ.web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Sede> Sedes { get; set; }
    public DbSet<Alojamiento> Alojamiento { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<TipoAlojamiento> TiposAlojamiento { get; set; }
    public DbSet<Temporada> Temporadas { get; set; }
    public DbSet<Tarifa> Tarifas { get; set; }
    public DbSet<Disponibilidad> Disponibilidad { get; set; }
    public DbSet<ReservaDetalle> ReservaDetalles { get; set; }
}

