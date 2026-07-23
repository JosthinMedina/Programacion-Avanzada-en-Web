using System.ComponentModel.DataAnnotations;

namespace SistemaGestionAcademica.Web.Models
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un usuario.")]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El asunto es obligatorio.")]
        [StringLength(
            150,
            ErrorMessage = "El asunto no puede superar los 150 caracteres."
        )]
        public string Asunto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        [StringLength(
            1000,
            ErrorMessage = "El mensaje no puede superar los 1000 caracteres."
        )]
        public string Mensaje { get; set; } = string.Empty;

        [Display(Name = "Fecha de envío")]
        public DateTime FechaEnvio { get; set; }

        [Display(Name = "Estado de envío")]
        public bool EstadoEnvio { get; set; }
    }
}
