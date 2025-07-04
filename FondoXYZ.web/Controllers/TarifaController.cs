using FondoXYZ.web.Data;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.web.Controllers;

[Authorize]
public class TarifaController(ApplicationDbContext context, ITarifaService tarifaService) : Controller
{
    public async Task<IActionResult> ConsultarTarifa(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas, bool servicioLavanderia)
    {

        var alojamientos = await context.Alojamiento.ToListAsync();
        ViewBag.Alojamientos = alojamientos;

        // Llamada para obtener el precio total
        var precioTotal = await tarifaService.ConsultarTarifaAsync(alojamientoId, fechaInicio, fechaFin, numPersonas, servicioLavanderia);

        // precio total a la vista
        ViewBag.PrecioTotal = precioTotal;

        return View();
    }

}
