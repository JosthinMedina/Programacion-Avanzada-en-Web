using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface IEventoService
    {
        Task<int> RegistrarEvento(Evento evento);

        Task<IEnumerable<Evento>> ConsultarEventos();

        Task<Evento?> ConsultarEvento(int idEvento);

        Task<int> ActualizarEvento(Evento evento);

        Task<int> EliminarEvento(int idEvento);
    }
}
