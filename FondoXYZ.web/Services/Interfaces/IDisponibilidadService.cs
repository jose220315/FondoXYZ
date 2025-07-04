using FondoXYZ.web.Models;

namespace FondoXYZ.web.Services.Interfaces
{
    public interface IDisponibilidadService
    {
        Task<List<Alojamiento>> ConsultarDisponibilidad(DateTime fechaInicio, DateTime fechaFin);
    }
}
