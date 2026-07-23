using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface IEstudianteService
    {
        Task<int> RegistrarEstudiante(Estudiante estudiante);

        Task<IEnumerable<Estudiante>> ConsultarEstudiantes();

        Task<Estudiante?> ConsultarEstudiante(int idEstudiante);

        Task<int> ActualizarEstudiante(Estudiante estudiante);

        Task<int> EliminarEstudiante(int idEstudiante);
    }
}
