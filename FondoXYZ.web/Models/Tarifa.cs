namespace FondoXYZ.web.Models
{
    public class Tarifa
    {
        public int Id { get; set; }

        public int AlojamientoId { get; set; }
        public Alojamiento Alojamiento { get; set; }

        public int TemporadaId { get; set; }
        public Temporada Temporada { get; set; }

        public decimal PrecioBase { get; set; }
        public decimal PrecioPersonaAdicional { get; set; }
        public int MaxPersonas { get; set; }
    }
}
