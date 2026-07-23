using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface ICalificacionService
    {
        Task<int> RegistrarCalificacion(Calificacion calificacion);

        Task<IEnumerable<Calificacion>> ConsultarCalificaciones();

        Task<Calificacion?> ConsultarCalificacion(int idCalificacion);

        Task<int> ActualizarCalificacion(Calificacion calificacion);

        Task<int> EliminarCalificacion(int idCalificacion);
    }
}
