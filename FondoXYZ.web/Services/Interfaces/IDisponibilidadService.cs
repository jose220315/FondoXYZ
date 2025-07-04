using FondoXYZ.web.Services.Dto;

namespace FondoXYZ.web.Services.Interfaces
{
    public interface IDisponibilidadService
    {
        Task<List<AlojamientoDto>> ConsultarDisponibilidad(DateTime fechaInicio, DateTime fechaFin);
    }
}
