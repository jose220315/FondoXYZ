namespace FondoXYZ.web.Models
{
    public class Alojamiento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int SedeId { get; set; }
        public int TipoAlojamientoId { get; set; }
        public int NumHabitaciones { get; set; }
        public int Capacidad { get; set; }

        public Sede Sede { get; set; }
        public TipoAlojamiento TipoAlojamiento { get; set; }
    }

}
