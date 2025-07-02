namespace FondoXYZ.web.Models
{
    public class ReservaDetalle
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }
        public int AlojamientoId { get; set; }
        public int NumPersonas { get; set; }
        public decimal Total { get; set; }

        public Reserva Reserva { get; set; }
        public Alojamiento Alojamiento { get; set; }
    }

}
