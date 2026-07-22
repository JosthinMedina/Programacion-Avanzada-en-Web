namespace SistemaGestionAcademica.API.Entities
{
    public class Evento
    {
        public int IdEvento { get; set; }

        public int IdCurso { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; }
    }
}
