namespace SistemaGestionAcademica.API.Entities
{
    public class Curso
    {
        public int IdCurso { get; set; }

        public int IdProfesor { get; set; }

        public string NombreCurso { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
