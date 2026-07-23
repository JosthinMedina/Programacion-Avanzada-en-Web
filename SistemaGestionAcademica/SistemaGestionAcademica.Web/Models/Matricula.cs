using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Matricula
    {
        public int IdMatricula { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estudiante.")]
        [Display(Name = "Estudiante")]
        public int IdEstudiante { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un curso.")]
        [Display(Name = "Curso")]
        public int IdCurso { get; set; }

        [Display(Name = "Fecha de matrícula")]
        public DateTime FechaMatricula { get; set; }

        public bool Estado { get; set; }
    }
}
