using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.API.Entities;
using SistemaGestionAcademica.API.Interfaces;

namespace SistemaGestionAcademica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarUsuarios()
        {
            var resultado = await _usuarioService.ConsultarUsuarios();
            return Ok(resultado);
        }

        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> ConsultarUsuario(int idUsuario)
        {
            var resultado = await _usuarioService.ConsultarUsuario(idUsuario);

            if (resultado == null)
            {
                return NotFound("El usuario no existe.");
            }

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario(Usuario usuario)
        {
            var resultado = await _usuarioService.RegistrarUsuario(usuario);

            if (resultado == -1)
            {
                return BadRequest("Ya existe un usuario con ese correo.");
            }

            return Ok(new
            {
                Mensaje = "Usuario registrado correctamente.",
                IdUsuario = resultado
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarUsuario(Usuario usuario)
        {
            var resultado = await _usuarioService.ActualizarUsuario(usuario);

            if (resultado == -1)
            {
                return BadRequest("Ya existe otro usuario con ese correo.");
            }

            return Ok("Usuario actualizado correctamente.");
        }

        [HttpDelete("{idUsuario}")]
        public async Task<IActionResult> EliminarUsuario(int idUsuario)
        {
            await _usuarioService.EliminarUsuario(idUsuario);

            return Ok("Usuario desactivado correctamente.");
        }
    }
}
