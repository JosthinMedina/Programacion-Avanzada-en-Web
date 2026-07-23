using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UsuariosController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private HttpClient CrearCliente()
        {
            var cliente = _httpClientFactory.CreateClient();

            cliente.BaseAddress = new Uri(
                _configuration["ApiSettings:BaseUrl"]!
            );

            return cliente;
        }

        // ===========================
        // LISTAR
        // ===========================

        public async Task<IActionResult> Index()
        {
            var cliente = CrearCliente();

            var usuarios = await cliente.GetFromJsonAsync<List<Usuario>>("Usuarios");

            return View(usuarios ?? new List<Usuario>());
        }

        // ===========================
        // REGISTRAR
        // ===========================

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            var cliente = CrearCliente();

            var respuesta = await cliente.PostAsJsonAsync("Usuarios", usuario);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] = "Usuario registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "No fue posible registrar el usuario.";

            return View(usuario);
        }

        // ===========================
        // EDITAR
        // ===========================

        public async Task<IActionResult> Editar(int id)
        {
            var cliente = CrearCliente();

            var usuario = await cliente.GetFromJsonAsync<Usuario>($"Usuarios/{id}");

            if (usuario == null)
                return RedirectToAction(nameof(Index));

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            var cliente = CrearCliente();

            var respuesta = await cliente.PutAsJsonAsync("Usuarios", usuario);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] = "Usuario actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "No fue posible actualizar el usuario.";

            return View(usuario);
        }

        // ===========================
        // ELIMINAR
        // ===========================

        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = CrearCliente();

            var respuesta = await cliente.DeleteAsync($"Usuarios/{id}");

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] = "Usuario desactivado correctamente.";
            }
            else
            {
                TempData["Error"] = "No fue posible desactivar el usuario.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
