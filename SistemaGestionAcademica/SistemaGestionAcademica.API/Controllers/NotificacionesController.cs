using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;

        public NotificacionesController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarNotificaciones()
        {
            var resultado = await _notificacionService.ConsultarNotificaciones();
            return Ok(resultado);
        }

        [HttpGet("{idNotificacion}")]
        public async Task<IActionResult> ConsultarNotificacion(int idNotificacion)
        {
            var resultado = await _notificacionService.ConsultarNotificacion(idNotificacion);

            if (resultado == null)
                return NotFound("La notificación no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarNotificacion(Notificacion notificacion)
        {
            var resultado = await _notificacionService.RegistrarNotificacion(notificacion);

            if (resultado == -1)
                return BadRequest("El usuario indicado no existe o está inactivo.");

            return Ok(new
            {
                Mensaje = "Notificación registrada correctamente.",
                IdNotificacion = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarNotificacion(Notificacion notificacion)
        {
            var resultado = await _notificacionService.ActualizarNotificacion(notificacion);

            if (resultado == -1)
                return BadRequest("El usuario indicado no existe o está inactivo.");

            if (resultado == 0)
                return NotFound("La notificación no existe.");

            return Ok("Notificación actualizada correctamente.");
        }

        [HttpDelete("{idNotificacion}")]
        public async Task<IActionResult> EliminarNotificacion(int idNotificacion)
        {
            var resultado = await _notificacionService.EliminarNotificacion(idNotificacion);

            if (resultado == 0)
                return NotFound("La notificación no existe.");

            return Ok("Notificación eliminada correctamente.");
        }
    }
}