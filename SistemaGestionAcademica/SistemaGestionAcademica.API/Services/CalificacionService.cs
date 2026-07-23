using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class CalificacionService : ICalificacionService
    {
        private readonly IConfiguration _configuration;

        public CalificacionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarCalificacion(Calificacion calificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_matricula", calificacion.IdMatricula);
            parametros.Add("@nota", calificacion.Nota);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarCalificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Calificacion>> ConsultarCalificaciones()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Calificacion>(
                "spConsultarCalificaciones",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Calificacion?> ConsultarCalificacion(int idCalificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_calificacion", idCalificacion);

            return await conexion.QueryFirstOrDefaultAsync<Calificacion>(
                "spConsultarCalificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarCalificacion(Calificacion calificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_calificacion", calificacion.IdCalificacion);
            parametros.Add("@id_matricula", calificacion.IdMatricula);
            parametros.Add("@nota", calificacion.Nota);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarCalificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarCalificacion(int idCalificacion)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_calificacion", idCalificacion);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarCalificacion",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
