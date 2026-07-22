namespace SistemaGestionAcademica.API.Entities
{
    public class Profesor
    {
        public int IdProfesor { get; set; }

        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string PrimerApellido { get; set; } = string.Empty;

        public string SegundoApellido { get; set; } = string.Empty;

        public string Identificacion { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Especialidad { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
