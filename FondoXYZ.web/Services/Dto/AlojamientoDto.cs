namespace FondoXYZ.web.Services.Dto;
    public record AlojamientoDto(
        int Id,
        string Nombre,
        int SedeId,
        int TipoAlojamientoId,
        int NumHabitaciones,
        int Capacidad
    );
