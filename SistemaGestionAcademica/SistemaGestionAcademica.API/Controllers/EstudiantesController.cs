using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;

        public EstudiantesController(IEstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarEstudiantes()
        {
            var resultado = await _estudianteService.ConsultarEstudiantes();
            return Ok(resultado);
        }

        [HttpGet("{idEstudiante}")]
        public async Task<IActionResult> ConsultarEstudiante(int idEstudiante)
        {
            var resultado = await _estudianteService.ConsultarEstudiante(idEstudiante);

            if (resultado == null)
            {
                return NotFound("El estudiante no existe.");
            }

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEstudiante(Estudiante estudiante)
        {
            var resultado = await _estudianteService.RegistrarEstudiante(estudiante);

            if (resultado == -1)
            {
                return BadRequest("Ya existe un estudiante con esa identificación.");
            }

            return Ok(new
            {
                Mensaje = "Estudiante registrado correctamente.",
                IdEstudiante = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarEstudiante(Estudiante estudiante)
        {
            var resultado = await _estudianteService.ActualizarEstudiante(estudiante);

            if (resultado == -1)
            {
                return BadRequest("Ya existe otro estudiante con esa identificación.");
            }

            return Ok("Estudiante actualizado correctamente.");
        }

        [HttpDelete("{idEstudiante}")]
        public async Task<IActionResult> EliminarEstudiante(int idEstudiante)
        {
            await _estudianteService.EliminarEstudiante(idEstudiante);

            return Ok("Estudiante eliminado correctamente.");
        }
    }
}