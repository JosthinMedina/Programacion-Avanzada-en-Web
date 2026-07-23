using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface IProfesorService
    {
        Task<int> RegistrarProfesor(Profesor profesor);

        Task<IEnumerable<Profesor>> ConsultarProfesores();

        Task<Profesor?> ConsultarProfesor(int idProfesor);

        Task<int> ActualizarProfesor(Profesor profesor);

        Task<int> EliminarProfesor(int idProfesor);
    }
}
