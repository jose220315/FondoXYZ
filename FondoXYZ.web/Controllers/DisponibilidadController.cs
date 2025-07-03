using FondoXYZ.web.Data;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace FondoXYZ.web.Controllers
{ 
    [Authorize]
    public class DisponibilidadController : Controller
    {
        private readonly IDisponibilidadService _disponibilidadService;

        public DisponibilidadController(IDisponibilidadService disponibilidadService)
        {
            _disponibilidadService = disponibilidadService;
        }

        public IActionResult Consultar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Consultar(DateTime fechaInicio, DateTime fechaFin)
        {
            var disponibles = await _disponibilidadService.ConsultarDisponibilidad(fechaInicio, fechaFin);
            ViewBag.Resultado = disponibles;
            return View();
        }
    }

}
