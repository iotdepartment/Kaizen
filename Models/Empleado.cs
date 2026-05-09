using System.ComponentModel.DataAnnotations;

namespace Kaizen.Models
{
    public class Empleado
    {
        [Key]
        public int NoEmpleado { get; set; }

        [Required]
        public string Nombre { get; set; }
    }

}
