using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Profesor
    {
        public int IdProfesor { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Display(Name = "Segundo Apellido")]
        public string? SegundoApellido { get; set; }

        [Required]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        [Required]
        public string Especialidad { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
