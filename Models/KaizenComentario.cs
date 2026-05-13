using System;
using System.ComponentModel.DataAnnotations;

namespace Kaizen.Models
{
    public class KaizenComentario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdeaId { get; set; } // FK a KaizenIdea

        [Required]
        public string Comentario { get; set; } = string.Empty;

        public DateTime FechaComentario { get; set; } = DateTime.Now;

        public int NumeroComentario { get; set; }

        public string? Usuario { get; set; } // aquí se guarda el nombre del usuario

        // Navegación
        public KaizenIdea Idea { get; set; }
    }
}