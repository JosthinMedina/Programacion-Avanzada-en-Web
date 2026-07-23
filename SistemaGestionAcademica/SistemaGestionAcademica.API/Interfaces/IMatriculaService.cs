using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface IMatriculaService
    {
        Task<int> RegistrarMatricula(Matricula matricula);

        Task<IEnumerable<Matricula>> ConsultarMatriculas();

        Task<Matricula?> ConsultarMatricula(int idMatricula);

        Task<int> ActualizarMatricula(Matricula matricula);

        Task<int> EliminarMatricula(int idMatricula);
    }
}
