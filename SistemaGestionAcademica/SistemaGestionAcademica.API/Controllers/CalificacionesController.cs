using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionService _calificacionService;

        public CalificacionesController(ICalificacionService calificacionService)
        {
            _calificacionService = calificacionService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarCalificaciones()
        {
            var resultado = await _calificacionService.ConsultarCalificaciones();
            return Ok(resultado);
        }

        [HttpGet("{idCalificacion}")]
        public async Task<IActionResult> ConsultarCalificacion(int idCalificacion)
        {
            var resultado = await _calificacionService.ConsultarCalificacion(idCalificacion);

            if (resultado == null)
                return NotFound("La calificación no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCalificacion(Calificacion calificacion)
        {
            if (calificacion.Nota < 0 || calificacion.Nota > 100)
                return BadRequest("La nota debe estar entre 0 y 100.");

            var resultado = await _calificacionService.RegistrarCalificacion(calificacion);

            if (resultado == -1)
                return BadRequest("La matrícula no existe o ya tiene una calificación registrada.");

            return Ok(new
            {
                Mensaje = "Calificación registrada correctamente.",
                IdCalificacion = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarCalificacion(Calificacion calificacion)
        {
            if (calificacion.Nota < 0 || calificacion.Nota > 100)
                return BadRequest("La nota debe estar entre 0 y 100.");

            var resultado = await _calificacionService.ActualizarCalificacion(calificacion);

            if (resultado == 0)
                return NotFound("La calificación no existe.");

            return Ok("Calificación actualizada correctamente.");
        }

        [HttpDelete("{idCalificacion}")]
        public async Task<IActionResult> EliminarCalificacion(int idCalificacion)
        {
            var resultado = await _calificacionService.EliminarCalificacion(idCalificacion);

            if (resultado == 0)
                return NotFound("La calificación no existe.");

            return Ok("Calificación eliminada correctamente.");
        }
    }
}