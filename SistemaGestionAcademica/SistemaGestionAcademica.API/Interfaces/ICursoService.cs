using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface ICursoService
    {
        Task<int> RegistrarCurso(Curso curso);

        Task<IEnumerable<Curso>> ConsultarCursos();

        Task<Curso?> ConsultarCurso(int idCurso);

        Task<int> ActualizarCurso(Curso curso);

        Task<int> EliminarCurso(int idCurso);
    }
}
