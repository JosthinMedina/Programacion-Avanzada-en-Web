using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un curso.")]
        [Display(Name = "Curso")]
        public int IdCurso { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
        [Display(Name = "Fecha de finalización")]
        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; }
    }
}
