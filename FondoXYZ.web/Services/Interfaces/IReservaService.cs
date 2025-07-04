namespace FondoXYZ.web.Services.Interfaces
{
    public interface IReservaService
    {
        Task<decimal> CalcularTotalReservaAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas);
        Task<bool> VerificarDisponibilidadAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin);
        Task CrearReservaAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas, string usuarioId);
        Task CancelarReservaAsync(int reservaId);
    }

}
