using System.ComponentModel.DataAnnotations;

namespace spi.Models
{
    public class Usuario
    {


        [Key]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apaterno { get; set; }
        public string Amaterno { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        public string Role { get; set; }


        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
    }


}
