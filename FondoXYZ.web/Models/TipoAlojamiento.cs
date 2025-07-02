namespace FondoXYZ.web.Models
{
    public class TipoAlojamiento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<Alojamiento> Alojamientos { get; set; }
    }

}
