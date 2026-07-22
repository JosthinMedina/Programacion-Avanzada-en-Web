namespace SistemaGestionAcademica.API.Entities
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }

        public int IdUsuario { get; set; }

        public string Asunto { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        public bool EstadoEnvio { get; set; }
    }
}
