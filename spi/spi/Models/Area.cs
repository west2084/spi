using System.ComponentModel.DataAnnotations;

namespace spi.Models
{
    public class Area
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(50)]
        public string des_area { get; set; }

        [Required]
        [StringLength(30)]
        public string siglas_area { get; set; }

        public ICollection<ObservacionArea> ObservacionAreas { get; set; } = new List<ObservacionArea>();
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
