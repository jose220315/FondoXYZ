namespace FondoXYZ.web.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public decimal TotalReserva { get; set; }

        public ApplicationUser Usuario { get; set; }
        public ICollection<ReservaDetalle> Detalles { get; set; }
    }

}
