using System.ComponentModel.DataAnnotations;

namespace Kaizen.Models
{
    public class KaizenIdea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NoEmpleado { get; set; }

        public Empleado Empleado { get; set; }   // Navegación

        [Required]
        public string Supervisor { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public int Turno { get; set; }

        [Required]
        public string AreaAplicacion { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public string ImagenActualBase64 { get; set; }
        public string ImagenMejoradaBase64 { get; set; }
    }

}
