namespace FondoXYZ.web.Models
{
    public class Temporada
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public ICollection<Tarifa> Tarifas { get; set; }
    }
}
