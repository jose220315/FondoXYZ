namespace FondoXYZ.web.Services.Interfaces
{
    public interface ITarifaService
    {
        Task<decimal> ConsultarTarifaAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas, bool servicioLavanderia);
    }
}
