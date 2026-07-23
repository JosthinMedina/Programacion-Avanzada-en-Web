using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesoresController : ControllerBase
    {
        private readonly IProfesorService _profesorService;

        public ProfesoresController(IProfesorService profesorService)
        {
            _profesorService = profesorService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarProfesores()
        {
            var resultado = await _profesorService.ConsultarProfesores();
            return Ok(resultado);
        }

        [HttpGet("{idProfesor}")]
        public async Task<IActionResult> ConsultarProfesor(int idProfesor)
        {
            var resultado = await _profesorService.ConsultarProfesor(idProfesor);

            if (resultado == null)
                return NotFound("El profesor no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarProfesor(Profesor profesor)
        {
            var resultado = await _profesorService.RegistrarProfesor(profesor);

            if (resultado == -1)
                return BadRequest("Ya existe un profesor con esa identificación o usuario.");

            return Ok(new
            {
                Mensaje = "Profesor registrado correctamente.",
                IdProfesor = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarProfesor(Profesor profesor)
        {
            var resultado = await _profesorService.ActualizarProfesor(profesor);

            if (resultado == -1)
                return BadRequest("Ya existe otro profesor con esa identificación o usuario.");

            if (resultado == 0)
                return NotFound("El profesor no existe.");

            return Ok("Profesor actualizado correctamente.");
        }

        [HttpDelete("{idProfesor}")]
        public async Task<IActionResult> EliminarProfesor(int idProfesor)
        {
            var resultado = await _profesorService.EliminarProfesor(idProfesor);

            if (resultado == 0)
                return NotFound("El profesor no existe.");

            return Ok("Profesor eliminado correctamente.");
        }
    }
}