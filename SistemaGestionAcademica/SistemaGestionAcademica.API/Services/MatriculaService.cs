using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IConfiguration _configuration;

        public MatriculaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarMatricula(Matricula matricula)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_estudiante", matricula.IdEstudiante);
            parametros.Add("@id_curso", matricula.IdCurso);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarMatricula",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Matricula>> ConsultarMatriculas()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Matricula>(
                "spConsultarMatriculas",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Matricula?> ConsultarMatricula(int idMatricula)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_matricula", idMatricula);

            return await conexion.QueryFirstOrDefaultAsync<Matricula>(
                "spConsultarMatricula",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarMatricula(Matricula matricula)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_matricula", matricula.IdMatricula);
            parametros.Add("@id_estudiante", matricula.IdEstudiante);
            parametros.Add("@id_curso", matricula.IdCurso);
            parametros.Add("@estado", matricula.Estado);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarMatricula",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarMatricula(int idMatricula)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_matricula", idMatricula);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarMatricula",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}