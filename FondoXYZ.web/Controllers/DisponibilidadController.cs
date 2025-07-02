using FondoXYZ.web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.web.Controllers
{ 
    [Authorize]
    public class DisponibilidadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisponibilidadController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Consultar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Consultar(DateTime fechaInicio, DateTime fechaFin)
        {
            var disponibles = await _context.Alojamiento
                .FromSqlRaw("EXEC sp_HabitacionesDisponiblesPorFecha @FechaInicio, @FechaFin",
                    new SqlParameter("@FechaInicio", fechaInicio),
                    new SqlParameter("@FechaFin", fechaFin))
                .ToListAsync();

            ViewBag.Resultado = disponibles;
            return View();
        }
    }
}
