using FondoXYZ.web.Data;
using FondoXYZ.web.Models;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.web.Services
{
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly ApplicationDbContext _context;

        public DisponibilidadService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Alojamiento>> ConsultarDisponibilidad(DateTime fechaInicio, DateTime fechaFin)
        {
            var disponibles = await _context.Alojamiento
                .FromSqlRaw("EXEC sp_HabitacionesDisponiblesPorFecha @FechaInicio, @FechaFin",
                    new SqlParameter("@FechaInicio", fechaInicio),
                    new SqlParameter("@FechaFin", fechaFin))
                .ToListAsync();

            return disponibles;
        }
    }
}
