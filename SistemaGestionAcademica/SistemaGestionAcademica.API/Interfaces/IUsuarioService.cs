using SistemaGestionAcademica.API.Entities;

namespace SistemaGestionAcademica.API.Interfaces
{
    public interface IUsuarioService
    {
        Task<int> RegistrarUsuario(Usuario usuario);

        Task<IEnumerable<Usuario>> ConsultarUsuarios();

        Task<Usuario?> ConsultarUsuario(int idUsuario);

        Task<int> ActualizarUsuario(Usuario usuario);

        Task<int> EliminarUsuario(int idUsuario);
    }
}
