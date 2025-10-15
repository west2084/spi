using System.ComponentModel.DataAnnotations;

namespace spi.Models
{
    public class Evidencias
    {


        [Key]
        public int Id { get; set; }

        public string NombreOriginal { get; set; }
        public string RutaArchivo { get; set; }
        public DateTime FechaSubida { get; set; }


        public int ObservacionesId { get; set; }
        public virtual Observaciones Observaciones { get; set; }



    }
}
