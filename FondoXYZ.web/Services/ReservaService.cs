using FondoXYZ.web.Data;
using FondoXYZ.web.Models;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FondoXYZ.web.Services;

public class ReservaService(ApplicationDbContext context) : IReservaService
{
    public async Task<decimal> CalcularTotalReservaAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas)
    {
       
        var query = "EXEC sp_CalcularTotalReserva @AlojamientoId, @FechaInicio, @FechaFin, @NumPersonas";

        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;
        command.Parameters.Add(new SqlParameter("@AlojamientoId", alojamientoId));
        command.Parameters.Add(new SqlParameter("@FechaInicio", fechaInicio));
        command.Parameters.Add(new SqlParameter("@FechaFin", fechaFin));
        command.Parameters.Add(new SqlParameter("@NumPersonas", numPersonas));

        if (command.Connection.State != System.Data.ConnectionState.Open)
        {
            await command.Connection.OpenAsync();
        }

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return reader.GetDecimal(reader.GetOrdinal("TotalReserva"));
        }
        return 0;
    }

    public async Task<bool> VerificarDisponibilidadAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin)
    {
        var fechasOcupadas = await context.Disponibilidad
            .Where(d => d.AlojamientoId == alojamientoId && d.Fecha >= fechaInicio && d.Fecha <= fechaFin)
            .AnyAsync();

        return fechasOcupadas;
    }

    public async Task CrearReservaAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas, string usuarioId)
    {
        var reserva = new Reserva
        {
            UsuarioId = Guid.Parse(usuarioId),
            FechaReserva = DateTime.Now,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            Estado = "Activa"
        };

        context.Reservas.Add(reserva);
        await context.SaveChangesAsync();

        var totalReserva = await CalcularTotalReservaAsync(alojamientoId, fechaInicio, fechaFin, numPersonas);

        var detalle = new ReservaDetalle
        {
            ReservaId = reserva.Id,
            AlojamientoId = alojamientoId,
            NumPersonas = numPersonas,
            TotalReserva = totalReserva
        };

        context.ReservaDetalles.Add(detalle);
        await context.SaveChangesAsync();

        var fecha = fechaInicio;
        while (fecha <= fechaFin)
        {
            context.Disponibilidad.Add(new Disponibilidad
            {
                AlojamientoId = alojamientoId,
                Fecha = fecha,
                ReservaId = reserva.Id
            });

            fecha = fecha.AddDays(1);
        }

        await context.SaveChangesAsync();
    }

    public async Task CancelarReservaAsync(int reservaId)
    {

        var reserva = await context.Reservas.FindAsync(reservaId);

        if (reserva != null)
        {

            reserva.Estado = "Cancelada";

            context.Reservas.Update(reserva);

            var fechasOcupadas = await context.Disponibilidad
                .Where(d => d.ReservaId == reservaId)
                .ToListAsync();

            context.Disponibilidad.RemoveRange(fechasOcupadas);

            await context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Reserva no encontrada.");
        }
    }
}
