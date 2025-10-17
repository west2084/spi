using System.ComponentModel.DataAnnotations;

namespace spi.Models
{
    public class Observaciones
    {


        [Key]
        public int Id { get; set; }


     
        [StringLength(30)]
        public string cve_obs { get; set; }


        [Required]
        [StringLength(255)]
        public string des_obs { get; set; }

        [Required]
        [StringLength(255)]
        public string indicador { get; set; }


        [Required]
        
        public DateTime fecha_obs { get; set; }

        [Required]
        public DateTime fecha_cumplimiento { get; set; }


        [Required]
        public DateTime fecha_ult_act { get; set; }

        [Required]
        public int valor_cumplimiento { get; set; }
        [Required]
        public int valor_meta { get; set; }


        [Required]
        public string estatus { get; set; }

        public int ProyectoId { get; set; }
        public virtual Proyecto Proyecto { get; set; }

        // Para el filtrado por usuario y requiere migración en UsuarioId de base de datos
        // public int UsuarioId { get; set; }
        // public virtual Usuario Usuario { get; set; }


        public int AreaId { get; set; }
        public virtual Area Area { get; set; }

        // Eliminamos AreaId/Area porque ahora será muchos a muchos
        public ICollection<ObservacionArea> ObservacionAreas { get; set; } = new List<ObservacionArea>();


        public ICollection<Evidencias> Evidencias { get; set; } = new List<Evidencias>();

    }



}
