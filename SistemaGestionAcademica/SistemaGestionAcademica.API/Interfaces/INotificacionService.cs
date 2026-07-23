using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface INotificacionService
    {
        Task<int> RegistrarNotificacion(Notificacion notificacion);

        Task<IEnumerable<Notificacion>> ConsultarNotificaciones();

        Task<Notificacion?> ConsultarNotificacion(int idNotificacion);

        Task<int> ActualizarNotificacion(Notificacion notificacion);

        Task<int> EliminarNotificacion(int idNotificacion);
    }
}
