using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly IConfiguration _configuration;

        public ProfesorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarProfesor(Profesor profesor)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_usuario", profesor.IdUsuario);
            parametros.Add("@nombre", profesor.Nombre);
            parametros.Add("@primer_apellido", profesor.PrimerApellido);
            parametros.Add("@segundo_apellido", profesor.SegundoApellido);
            parametros.Add("@identificacion", profesor.Identificacion);
            parametros.Add("@correo", profesor.Correo);
            parametros.Add("@telefono", profesor.Telefono);
            parametros.Add("@especialidad", profesor.Especialidad);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarProfesor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Profesor>> ConsultarProfesores()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Profesor>(
                "spConsultarProfesores",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Profesor?> ConsultarProfesor(int idProfesor)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_profesor", idProfesor);

            return await conexion.QueryFirstOrDefaultAsync<Profesor>(
                "spConsultarProfesor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarProfesor(Profesor profesor)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_profesor", profesor.IdProfesor);
            parametros.Add("@id_usuario", profesor.IdUsuario);
            parametros.Add("@nombre", profesor.Nombre);
            parametros.Add("@primer_apellido", profesor.PrimerApellido);
            parametros.Add("@segundo_apellido", profesor.SegundoApellido);
            parametros.Add("@identificacion", profesor.Identificacion);
            parametros.Add("@correo", profesor.Correo);
            parametros.Add("@telefono", profesor.Telefono);
            parametros.Add("@especialidad", profesor.Especialidad);
            parametros.Add("@estado", profesor.Estado);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarProfesor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarProfesor(int idProfesor)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_profesor", idProfesor);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarProfesor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}