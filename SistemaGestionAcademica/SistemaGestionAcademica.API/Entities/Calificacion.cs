namespace SistemaGestionAcademica.API.Entities
{
    public class Calificacion
    {
        public int IdCalificacion { get; set; }

        public int IdMatricula { get; set; }

        public decimal Nota { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}
