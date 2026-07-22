namespace SistemaGestionAcademica.API.Entities
{
    public class Matricula
    {
        public int IdMatricula { get; set; }

        public int IdEstudiante { get; set; }

        public int IdCurso { get; set; }

        public DateTime FechaMatricula { get; set; }

        public bool Estado { get; set; }
    }
}
