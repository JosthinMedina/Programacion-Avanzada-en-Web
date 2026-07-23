using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class CursoService : ICursoService
    {
        private readonly IConfiguration _configuration;

        public CursoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarCurso(Curso curso)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_profesor", curso.IdProfesor);
            parametros.Add("@nombre_curso", curso.NombreCurso);
            parametros.Add("@descripcion", curso.Descripcion);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarCurso",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Curso>> ConsultarCursos()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Curso>(
                "spConsultarCursos",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Curso?> ConsultarCurso(int idCurso)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_curso", idCurso);

            return await conexion.QueryFirstOrDefaultAsync<Curso>(
                "spConsultarCurso",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarCurso(Curso curso)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_curso", curso.IdCurso);
            parametros.Add("@id_profesor", curso.IdProfesor);
            parametros.Add("@nombre_curso", curso.NombreCurso);
            parametros.Add("@descripcion", curso.Descripcion);
            parametros.Add("@estado", curso.Estado);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarCurso",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarCurso(int idCurso)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_curso", idCurso);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarCurso",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
