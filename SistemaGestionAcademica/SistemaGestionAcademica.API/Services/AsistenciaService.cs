using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IConfiguration _configuration;

        public AsistenciaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarAsistencia(Asistencia asistencia)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_matricula", asistencia.IdMatricula);
            parametros.Add("@fecha", asistencia.Fecha);
            parametros.Add("@estado", asistencia.Estado);
            parametros.Add("@observacion", asistencia.Observacion);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarAsistencia",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Asistencia>> ConsultarAsistencias()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Asistencia>(
                "spConsultarAsistencias",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Asistencia?> ConsultarAsistencia(int idAsistencia)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_asistencia", idAsistencia);

            return await conexion.QueryFirstOrDefaultAsync<Asistencia>(
                "spConsultarAsistencia",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarAsistencia(Asistencia asistencia)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_asistencia", asistencia.IdAsistencia);
            parametros.Add("@id_matricula", asistencia.IdMatricula);
            parametros.Add("@fecha", asistencia.Fecha);
            parametros.Add("@estado", asistencia.Estado);
            parametros.Add("@observacion", asistencia.Observacion);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarAsistencia",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarAsistencia(int idAsistencia)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_asistencia", idAsistencia);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarAsistencia",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
