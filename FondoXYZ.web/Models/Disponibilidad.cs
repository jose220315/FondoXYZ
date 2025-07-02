namespace FondoXYZ.web.Models
{
    public class Disponibilidad
    {
        public int Id { get; set; }

        public int AlojamientoId { get; set; }
        public Alojamiento Alojamiento { get; set; }

        public DateTime Fecha { get; set; }

        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; }
    }

}
