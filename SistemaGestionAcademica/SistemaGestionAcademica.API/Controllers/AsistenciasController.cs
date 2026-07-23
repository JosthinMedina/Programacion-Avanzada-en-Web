using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;

        public AsistenciasController(IAsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarAsistencias()
        {
            var resultado = await _asistenciaService.ConsultarAsistencias();
            return Ok(resultado);
        }

        [HttpGet("{idAsistencia}")]
        public async Task<IActionResult> ConsultarAsistencia(int idAsistencia)
        {
            var resultado = await _asistenciaService.ConsultarAsistencia(idAsistencia);

            if (resultado == null)
                return NotFound("La asistencia no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAsistencia(Asistencia asistencia)
        {
            if (!EstadoValido(asistencia.Estado))
                return BadRequest("El estado debe ser Presente, Ausente o Justificada.");

            var resultado = await _asistenciaService.RegistrarAsistencia(asistencia);

            if (resultado == -1)
                return BadRequest("La matrícula no existe o ya tiene una asistencia registrada para esa fecha.");

            return Ok(new
            {
                Mensaje = "Asistencia registrada correctamente.",
                IdAsistencia = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarAsistencia(Asistencia asistencia)
        {
            if (!EstadoValido(asistencia.Estado))
                return BadRequest("El estado debe ser Presente, Ausente o Justificada.");

            var resultado = await _asistenciaService.ActualizarAsistencia(asistencia);

            if (resultado == -1)
                return BadRequest("Ya existe otra asistencia para esa matrícula y fecha.");

            if (resultado == 0)
                return NotFound("La asistencia no existe.");

            return Ok("Asistencia actualizada correctamente.");
        }

        [HttpDelete("{idAsistencia}")]
        public async Task<IActionResult> EliminarAsistencia(int idAsistencia)
        {
            var resultado = await _asistenciaService.EliminarAsistencia(idAsistencia);

            if (resultado == 0)
                return NotFound("La asistencia no existe.");

            return Ok("Asistencia eliminada correctamente.");
        }

        private static bool EstadoValido(string estado)
        {
            return estado == "Presente"
                || estado == "Ausente"
                || estado == "Justificada";
        }
    }
}