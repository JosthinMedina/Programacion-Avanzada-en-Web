using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarEventos()
        {
            var resultado = await _eventoService.ConsultarEventos();
            return Ok(resultado);
        }

        [HttpGet("{idEvento}")]
        public async Task<IActionResult> ConsultarEvento(int idEvento)
        {
            var resultado = await _eventoService.ConsultarEvento(idEvento);

            if (resultado == null)
                return NotFound("El evento no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEvento(Evento evento)
        {
            if (evento.FechaFin < evento.FechaInicio)
                return BadRequest("La fecha final no puede ser anterior a la fecha inicial.");

            var resultado = await _eventoService.RegistrarEvento(evento);

            if (resultado == -1)
                return BadRequest("El curso indicado no existe o está inactivo.");

            return Ok(new
            {
                Mensaje = "Evento registrado correctamente.",
                IdEvento = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarEvento(Evento evento)
        {
            if (evento.FechaFin < evento.FechaInicio)
                return BadRequest("La fecha final no puede ser anterior a la fecha inicial.");

            var resultado = await _eventoService.ActualizarEvento(evento);

            if (resultado == -1)
                return BadRequest("El curso indicado no existe o está inactivo.");

            if (resultado == 0)
                return NotFound("El evento no existe.");

            return Ok("Evento actualizado correctamente.");
        }

        [HttpDelete("{idEvento}")]
        public async Task<IActionResult> EliminarEvento(int idEvento)
        {
            var resultado = await _eventoService.EliminarEvento(idEvento);

            if (resultado == 0)
                return NotFound("El evento no existe.");

            return Ok("Evento eliminado correctamente.");
        }
    }
}