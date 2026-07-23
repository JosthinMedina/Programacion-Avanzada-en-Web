using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface IAsistenciaService
    {
        Task<int> RegistrarAsistencia(Asistencia asistencia);

        Task<IEnumerable<Asistencia>> ConsultarAsistencias();

        Task<Asistencia?> ConsultarAsistencia(int idAsistencia);

        Task<int> ActualizarAsistencia(Asistencia asistencia);

        Task<int> EliminarAsistencia(int idAsistencia);
    }
}