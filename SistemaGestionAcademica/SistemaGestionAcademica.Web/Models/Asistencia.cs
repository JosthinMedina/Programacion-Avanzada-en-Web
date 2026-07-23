using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Asistencia
    {
        public int IdAsistencia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una matrícula.")]
        [Display(Name = "Matrícula")]
        public int IdMatricula { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado.")]
        public string Estado { get; set; } = string.Empty;

        [Display(Name = "Observación")]
        public string? Observacion { get; set; }
    }
}
