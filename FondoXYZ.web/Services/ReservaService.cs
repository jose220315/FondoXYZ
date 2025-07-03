using FondoXYZ.web.Data;
using FondoXYZ.web.Models;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FondoXYZ.web.Services
{
    public class ReservaService : IReservaService
    {
        private readonly ApplicationDbContext _context;

        public ReservaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalcularTotalReservaAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas)
        {
           
            var query = "EXEC sp_CalcularTotalReserva @AlojamientoId, @FechaInicio, @FechaFin, @NumPersonas";

           
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add(new SqlParameter("@AlojamientoId", alojamientoId));
                command.Parameters.Add(new SqlParameter("@FechaInicio", fechaInicio));
                command.Parameters.Add(new SqlParameter("@FechaFin", fechaFin));
                command.Parameters.Add(new SqlParameter("@NumPersonas", numPersonas));

                
                if (command.Connection.State != System.Data.ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return reader.GetDecimal(reader.GetOrdinal("TotalReserva"));
                    }
                }
            }
            return 0;
        }

        public async Task<bool> VerificarDisponibilidadAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin)
        {
            var fechasOcupadas = await _context.Disponibilidad
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

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            var totalReserva = await CalcularTotalReservaAsync(alojamientoId, fechaInicio, fechaFin, numPersonas);

            var detalle = new ReservaDetalle
            {
                ReservaId = reserva.Id,
                AlojamientoId = alojamientoId,
                NumPersonas = numPersonas,
                TotalReserva = totalReserva
            };

            _context.ReservaDetalles.Add(detalle);
            await _context.SaveChangesAsync();

            var fecha = fechaInicio;
            while (fecha <= fechaFin)
            {
                _context.Disponibilidad.Add(new Disponibilidad
                {
                    AlojamientoId = alojamientoId,
                    Fecha = fecha,
                    ReservaId = reserva.Id
                });

                fecha = fecha.AddDays(1);
            }

            await _context.SaveChangesAsync();
        }

        public async Task CancelarReservaAsync(int reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva != null)
            {
                reserva.Estado = "Cancelada";
                _context.Update(reserva);

                var fechasOcupadas = await _context.Disponibilidad
                    .Where(d => d.ReservaId == reservaId)
                    .ToListAsync();

                _context.Disponibilidad.RemoveRange(fechasOcupadas);
                await _context.SaveChangesAsync();
            }
        }
    }

}
