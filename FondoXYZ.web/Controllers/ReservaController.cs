using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FondoXYZ.web.Data;
using FondoXYZ.web.Models;
using Microsoft.Data.SqlClient;

namespace FondoXYZ.web.Controllers
{
    [Authorize]
    public class ReservaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Crear()
        {
            ViewBag.Alojamiento = await _context.Alojamiento.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas)
        {
            var user = await _userManager.GetUserAsync(User);

            // Verificar disponibilidad de las fechas
            var fechasOcupadas = await _context.Disponibilidad
                .Where(d => d.AlojamientoId == alojamientoId && d.Fecha >= fechaInicio && d.Fecha <= fechaFin)
                .AnyAsync();

            if (fechasOcupadas)
            {
                // Si ya hay fechas ocupadas, redirigir al usuario con un mensaje de error
                TempData["ErrorMessage"] = "Las fechas seleccionadas ya están ocupadas.";
                return RedirectToAction("Crear");
            }

            // Crear la nueva reserva si está disponible
            var reserva = new Reserva
            {
                UsuarioId = Guid.Parse(user.Id),
                FechaReserva = DateTime.Now,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Estado = "Activa"
            };

            // Insertar la reserva en la base de datos
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // Calcular el total de la reserva utilizando el SP
            var totalReserva = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CalcularTotalReserva @AlojamientoId, @FechaInicio, @FechaFin, @NumPersonas",
                new SqlParameter("@AlojamientoId", alojamientoId),
                new SqlParameter("@FechaInicio", fechaInicio),
                new SqlParameter("@FechaFin", fechaFin),
                new SqlParameter("@NumPersonas", numPersonas)

            );

            var detalle = new ReservaDetalle
            {
                ReservaId = reserva.Id,
                AlojamientoId = alojamientoId,
                NumPersonas = numPersonas,
                TotalReserva = totalReserva // Cambiado para reflejar el valor calculado correctamente
            };

            // Insertar el detalle de la reserva
            _context.ReservaDetalles.Add(detalle);
            await _context.SaveChangesAsync();

            // Registrar las fechas ocupadas
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

            // Guardar las fechas ocupadas
            await _context.SaveChangesAsync();

            return RedirectToAction("MisReservas");
        }

        [HttpPost]
        public async Task<IActionResult> CancelarReserva(int reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);

            if (reserva != null)
            {
                reserva.Estado = "Cancelada";  // Cambiar estado de la reserva
                _context.Update(reserva);

                // Eliminar las fechas ocupadas asociadas a la reserva cancelada
                var fechasOcupadas = await _context.Disponibilidad
                    .Where(d => d.ReservaId == reservaId)
                    .ToListAsync();

                _context.Disponibilidad.RemoveRange(fechasOcupadas);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MisReservas");
        }




        [Authorize]
        public async Task<IActionResult> MisReservas()
        {
            var user = await _userManager.GetUserAsync(User);
            var reservas = await _context.Reservas
                .Include(r => r.Detalles)
                .ThenInclude(d => d.Alojamiento)
                .Where(r => r.UsuarioId == Guid.Parse(user.Id))
                .ToListAsync();

            return View(reservas);
        }


        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}
