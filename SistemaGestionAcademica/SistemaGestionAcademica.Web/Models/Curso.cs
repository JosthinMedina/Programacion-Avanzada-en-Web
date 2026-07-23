using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Curso
    {
        public int IdCurso { get; set; }

        [Required(ErrorMessage = "Debe indicar el profesor.")]
        [Display(Name = "Profesor")]
        public int IdProfesor { get; set; }

        [Required(ErrorMessage = "El nombre del curso es obligatorio.")]
        [Display(Name = "Nombre del curso")]
        public string NombreCurso { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        public bool Estado { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }
    }
}
