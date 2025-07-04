using AutoMapper;
using FondoXYZ.web.Data;
using FondoXYZ.web.Services.Dto;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.web.Services;

public class DisponibilidadService(ApplicationDbContext context, IMapper mapper) : IDisponibilidadService
{
    async Task<List<AlojamientoDto>> IDisponibilidadService.ConsultarDisponibilidad(DateTime fechaInicio, DateTime fechaFin)
    {
        var disponibles = await context.Alojamiento
           .FromSqlRaw("EXEC sp_HabitacionesDisponiblesPorFecha @FechaInicio, @FechaFin",
               new SqlParameter("@FechaInicio", fechaInicio),
               new SqlParameter("@FechaFin", fechaFin))
           .ToListAsync();

        return mapper.Map<List<AlojamientoDto>>(disponibles);
        
    }
}
