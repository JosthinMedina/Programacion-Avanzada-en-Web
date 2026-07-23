using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarMatriculas()
        {
            var resultado = await _matriculaService.ConsultarMatriculas();
            return Ok(resultado);
        }

        [HttpGet("{idMatricula}")]
        public async Task<IActionResult> ConsultarMatricula(int idMatricula)
        {
            var resultado = await _matriculaService.ConsultarMatricula(idMatricula);

            if (resultado == null)
                return NotFound("La matrícula no existe.");

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMatricula(Matricula matricula)
        {
            var resultado = await _matriculaService.RegistrarMatricula(matricula);

            if (resultado == -1)
                return BadRequest("El estudiante o el curso no existe, está inactivo o la matrícula ya existe.");

            return Ok(new
            {
                Mensaje = "Matrícula registrada correctamente.",
                IdMatricula = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarMatricula(Matricula matricula)
        {
            var resultado = await _matriculaService.ActualizarMatricula(matricula);

            if (resultado == -1)
                return BadRequest("El estudiante o el curso no existe, está inactivo o la matrícula está duplicada.");

            if (resultado == 0)
                return NotFound("La matrícula no existe.");

            return Ok("Matrícula actualizada correctamente.");
        }

        [HttpDelete("{idMatricula}")]
        public async Task<IActionResult> EliminarMatricula(int idMatricula)
        {
            var resultado = await _matriculaService.EliminarMatricula(idMatricula);

            if (resultado == 0)
                return NotFound("La matrícula no existe.");

            return Ok("Matrícula eliminada correctamente.");
        }
    }
}