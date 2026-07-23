using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursosController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarCursos()
        {
            var resultado = await _cursoService.ConsultarCursos();
            return Ok(resultado);
        }

        [HttpGet("{idCurso}")]
        public async Task<IActionResult> ConsultarCurso(int idCurso)
        {
            var resultado = await _cursoService.ConsultarCurso(idCurso);

            if (resultado == null)
                return NotFound("El curso no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCurso(Curso curso)
        {
            var resultado = await _cursoService.RegistrarCurso(curso);

            if (resultado == -1)
                return BadRequest("El profesor indicado no existe o está inactivo.");

            return Ok(new
            {
                Mensaje = "Curso registrado correctamente.",
                IdCurso = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarCurso(Curso curso)
        {
            var resultado = await _cursoService.ActualizarCurso(curso);

            if (resultado == -1)
                return BadRequest("El profesor indicado no existe o está inactivo.");

            if (resultado == 0)
                return NotFound("El curso no existe.");

            return Ok("Curso actualizado correctamente.");
        }

        [HttpDelete("{idCurso}")]
        public async Task<IActionResult> EliminarCurso(int idCurso)
        {
            var resultado = await _cursoService.EliminarCurso(idCurso);

            if (resultado == 0)
                return NotFound("El curso no existe.");

            return Ok("Curso eliminado correctamente.");
        }
    }
}