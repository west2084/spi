namespace spi.Models
{
    public class ObservacionArea
    {
        public int ObservacionesId { get; set; }
        public Observaciones Observaciones { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }

    }
}
