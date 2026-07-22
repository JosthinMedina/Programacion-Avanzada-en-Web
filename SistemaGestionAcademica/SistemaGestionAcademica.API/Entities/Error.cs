namespace SistemaGestionAcademica.API.Entities
{
    public class Error
    {
        public int IdError { get; set; }

        public int? IdUsuario { get; set; }

        public DateTime FechaError { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public string Origen { get; set; } = string.Empty;

        public string Detalle { get; set; } = string.Empty;
    }
}
