using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _configuration;

        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarUsuario(Usuario usuario)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_rol", usuario.IdRol);
            parametros.Add("@correo", usuario.Correo);
            parametros.Add("@contrasena", usuario.Contrasena);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarUsuario",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Usuario>> ConsultarUsuarios()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Usuario>(
                "spConsultarUsuarios",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Usuario?> ConsultarUsuario(int idUsuario)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_usuario", idUsuario);

            return await conexion.QueryFirstOrDefaultAsync<Usuario>(
                "spConsultarUsuario",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarUsuario(Usuario usuario)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_usuario", usuario.IdUsuario);
            parametros.Add("@id_rol", usuario.IdRol);
            parametros.Add("@correo", usuario.Correo);
            parametros.Add("@contrasena", usuario.Contrasena);
            parametros.Add("@estado", usuario.Estado);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarUsuario",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarUsuario(int idUsuario)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_usuario", idUsuario);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarUsuario",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
