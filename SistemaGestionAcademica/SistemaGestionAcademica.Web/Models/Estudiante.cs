using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Estudiante
    {
        public int IdEstudiante { get; set; }

        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string PrimerApellido { get; set; } = string.Empty;

        public string? SegundoApellido { get; set; }

        [Required]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public bool Estado { get; set; }
    }
}
