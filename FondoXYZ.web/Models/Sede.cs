namespace FondoXYZ.web.Models
{
    public class Sede
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public int CapacidadTotal { get; set; }

        public ICollection<Alojamiento> Alojamientos { get; set; }
    }

}
