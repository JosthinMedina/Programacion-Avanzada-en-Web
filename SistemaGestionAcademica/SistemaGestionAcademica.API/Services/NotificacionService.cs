using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly IConfiguration _configuration;

        public NotificacionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarNotificacion(Notificacion notificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_usuario", notificacion.IdUsuario);
            parametros.Add("@asunto", notificacion.Asunto);
            parametros.Add("@mensaje", notificacion.Mensaje);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarNotificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Notificacion>> ConsultarNotificaciones()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Notificacion>(
                "spConsultarNotificaciones",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Notificacion?> ConsultarNotificacion(int idNotificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_notificacion", idNotificacion);

            return await conexion.QueryFirstOrDefaultAsync<Notificacion>(
                "spConsultarNotificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarNotificacion(Notificacion notificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_notificacion", notificacion.IdNotificacion);
            parametros.Add("@id_usuario", notificacion.IdUsuario);
            parametros.Add("@asunto", notificacion.Asunto);
            parametros.Add("@mensaje", notificacion.Mensaje);
            parametros.Add("@estado_envio", notificacion.EstadoEnvio);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarNotificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarNotificacion(int idNotificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_notificacion", idNotificacion);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarNotificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
