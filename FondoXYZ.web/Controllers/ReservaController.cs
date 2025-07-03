using FondoXYZ.web.Data;
using FondoXYZ.web.Models;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ReservaController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IReservaService _reservaService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservaController(ApplicationDbContext context, IReservaService reservaService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _reservaService = reservaService;
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
        var usuarioId = user.Id;

        // Verificar disponibilidad de las fechas
        var fechasOcupadas = await _reservaService.VerificarDisponibilidadAsync(alojamientoId, fechaInicio, fechaFin);

        if (fechasOcupadas)
        {
            TempData["ErrorMessage"] = "Las fechas seleccionadas ya están ocupadas.";
            return RedirectToAction("Crear");
        }

        await _reservaService.CrearReservaAsync(alojamientoId, fechaInicio, fechaFin, numPersonas, usuarioId);

        return RedirectToAction("MisReservas");
    }

    [HttpPost]
    public async Task<IActionResult> CancelarReserva(int reservaId)
    {
        try
        {
            await _reservaService.CancelarReservaAsync(reservaId);

            TempData["SuccessMessage"] = "La reserva ha sido cancelada exitosamente.";
            return RedirectToAction("MisReservas");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Hubo un problema al cancelar la reserva: {ex.Message}";
            return RedirectToAction("MisReservas");
        }
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

