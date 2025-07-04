using FondoXYZ.web.Data;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.web.Controllers
{
    public class TarifaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITarifaService _tarifaService;

        public TarifaController(ApplicationDbContext context, ITarifaService tarifaService)
        {
            _context = context;
            _tarifaService = tarifaService;
        }

        public async Task<IActionResult> ConsultarTarifa(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas, bool servicioLavanderia)
        {

            var alojamientos = await _context.Alojamiento.ToListAsync();
            ViewBag.Alojamientos = alojamientos;

            // Llamada para obtener el precio total
            var precioTotal = await _tarifaService.ConsultarTarifaAsync(alojamientoId, fechaInicio, fechaFin, numPersonas, servicioLavanderia);

            // precio total a la vista
            ViewBag.PrecioTotal = precioTotal;

            return View();
        }

    }
}
