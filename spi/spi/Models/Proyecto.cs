using System.ComponentModel.DataAnnotations;

namespace spi.Models
{
    public class Proyecto
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(30)]
        public string des_proy { get; set; }


        [Required]
        [StringLength(30)]
        public string tipo_proy { get; set; }


        
        
        public DateTime fecha { get; set; }


        [Required]
        [StringLength(30)]
        public string estatus { get; set; }


        public ICollection<Observaciones> Observaciones { get; set; } = new List<Observaciones>();

    }
}
