using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IConfiguration _configuration;

        public EstudianteService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarEstudiante(
            Estudiante estudiante)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add(
                "@id_usuario",
                estudiante.IdUsuario
            );

            parametros.Add(
                "@nombre",
                estudiante.Nombre
            );

            parametros.Add(
                "@primer_apellido",
                estudiante.PrimerApellido
            );

            parametros.Add(
                "@segundo_apellido",
                estudiante.SegundoApellido
            );

            parametros.Add(
                "@identificacion",
                estudiante.Identificacion
            );

            parametros.Add(
                "@correo",
                estudiante.Correo
            );

            parametros.Add(
                "@telefono",
                estudiante.Telefono
            );

            parametros.Add(
                "@direccion",
                estudiante.Direccion
            );

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarEstudiante",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Estudiante>>
            ConsultarEstudiantes()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Estudiante>(
                "spConsultarEstudiantes",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Estudiante?>
            ConsultarEstudiante(int idEstudiante)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add(
                "@id_estudiante",
                idEstudiante
            );

            return await conexion
                .QueryFirstOrDefaultAsync<Estudiante>(
                    "spConsultarEstudiante",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );
        }

        public async Task<int> ActualizarEstudiante(
            Estudiante estudiante)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add(
                "@id_estudiante",
                estudiante.IdEstudiante
            );

            parametros.Add(
                "@id_usuario",
                estudiante.IdUsuario
            );

            parametros.Add(
                "@nombre",
                estudiante.Nombre
            );

            parametros.Add(
                "@primer_apellido",
                estudiante.PrimerApellido
            );

            parametros.Add(
                "@segundo_apellido",
                estudiante.SegundoApellido
            );

            parametros.Add(
                "@identificacion",
                estudiante.Identificacion
            );

            parametros.Add(
                "@correo",
                estudiante.Correo
            );

            parametros.Add(
                "@telefono",
                estudiante.Telefono
            );

            parametros.Add(
                "@direccion",
                estudiante.Direccion
            );

            parametros.Add(
                "@estado",
                estudiante.Estado
            );

            return await conexion.QuerySingleAsync<int>(
                "spActualizarEstudiante",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarEstudiante(
            int idEstudiante)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add(
                "@id_estudiante",
                idEstudiante
            );

            return await conexion.QuerySingleAsync<int>(
                "spEliminarEstudiante",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}