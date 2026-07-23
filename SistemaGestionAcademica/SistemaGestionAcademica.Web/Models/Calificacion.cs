using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Calificacion
    {
        public int IdCalificacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una matrícula.")]
        [Display(Name = "Matrícula")]
        public int IdMatricula { get; set; }

        [Required(ErrorMessage = "La nota es obligatoria.")]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
        public decimal Nota { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }
    }
}