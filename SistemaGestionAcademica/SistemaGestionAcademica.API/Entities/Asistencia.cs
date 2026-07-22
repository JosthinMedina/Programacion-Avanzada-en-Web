namespace SistemaGestionAcademica.API.Entities
{
    public class Asistencia
    {
        public int IdAsistencia { get; set; }

        public int IdMatricula { get; set; }

        public DateTime Fecha { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string? Observacion { get; set; }
    }
}
