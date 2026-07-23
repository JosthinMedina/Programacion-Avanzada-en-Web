using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Services
{
    public class EventoService : IEventoService
    {
        private readonly IConfiguration _configuration;

        public EventoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("ConexionSQL")
            );
        }

        public async Task<int> RegistrarEvento(Evento evento)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_curso", evento.IdCurso);
            parametros.Add("@titulo", evento.Titulo);
            parametros.Add("@descripcion", evento.Descripcion);
            parametros.Add("@fecha_inicio", evento.FechaInicio);
            parametros.Add("@fecha_fin", evento.FechaFin);

            return await conexion.QuerySingleAsync<int>(
                "spRegistrarEvento",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Evento>> ConsultarEventos()
        {
            using var conexion = CrearConexion();

            return await conexion.QueryAsync<Evento>(
                "spConsultarEventos",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Evento?> ConsultarEvento(int idEvento)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_evento", idEvento);

            return await conexion.QueryFirstOrDefaultAsync<Evento>(
                "spConsultarEvento",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarEvento(Evento evento)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();

            parametros.Add("@id_evento", evento.IdEvento);
            parametros.Add("@id_curso", evento.IdCurso);
            parametros.Add("@titulo", evento.Titulo);
            parametros.Add("@descripcion", evento.Descripcion);
            parametros.Add("@fecha_inicio", evento.FechaInicio);
            parametros.Add("@fecha_fin", evento.FechaFin);
            parametros.Add("@estado", evento.Estado);

            return await conexion.QuerySingleAsync<int>(
                "spActualizarEvento",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarEvento(int idEvento)
        {
            using var conexion = CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("@id_evento", idEvento);

            return await conexion.QuerySingleAsync<int>(
                "spEliminarEvento",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
