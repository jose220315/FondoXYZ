using FondoXYZ.web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.web.Controllers;

[Authorize]
public class DisponibilidadController(IDisponibilidadService disponibilidadService) : Controller
{
    public IActionResult Consultar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Consultar(DateTime fechaInicio, DateTime fechaFin)
    {
        var disponibles = await disponibilidadService.ConsultarDisponibilidad(fechaInicio, fechaFin);
        ViewBag.Resultado = disponibles;
        return View();
    }
}
