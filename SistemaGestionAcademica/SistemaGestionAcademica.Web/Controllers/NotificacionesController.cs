using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class NotificacionesController : Controller
    {
        private readonly HttpClient _http;

        public NotificacionesController(
            IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var notificaciones = await _http
                .GetFromJsonAsync<List<Notificacion>>(
                    "Notificaciones"
                );

            var usuarios = await _http
                .GetFromJsonAsync<List<Usuario>>(
                    "Usuarios"
                );

            ViewBag.Usuarios =
                usuarios ?? new List<Usuario>();

            return View(
                notificaciones ?? new List<Notificacion>()
            );
        }

        public async Task<IActionResult> Registrar()
        {
            await CargarUsuarios();

            return View(new Notificacion
            {
                FechaEnvio = DateTime.Now,
                EstadoEnvio = false
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(
            Notificacion notificacion)
        {
            if (!ModelState.IsValid)
            {
                await CargarUsuarios(
                    notificacion.IdUsuario
                );

                return View(notificacion);
            }

            var respuesta = await _http.PostAsJsonAsync(
                "Notificaciones",
                notificacion
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Notificación registrada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarUsuarios(
                notificacion.IdUsuario
            );

            return View(notificacion);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Notificaciones/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "La notificación seleccionada no existe.";

                return RedirectToAction(nameof(Index));
            }

            var notificacion = await respuesta.Content
                .ReadFromJsonAsync<Notificacion>();

            if (notificacion == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await CargarUsuarios(
                notificacion.IdUsuario
            );

            return View(notificacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            Notificacion notificacion)
        {
            if (!ModelState.IsValid)
            {
                await CargarUsuarios(
                    notificacion.IdUsuario
                );

                return View(notificacion);
            }

            var respuesta = await _http.PutAsJsonAsync(
                "Notificaciones",
                notificacion
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Notificación actualizada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarUsuarios(
                notificacion.IdUsuario
            );

            return View(notificacion);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _http.DeleteAsync(
                $"Notificaciones/{id}"
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Notificación eliminada correctamente.";
            }
            else
            {
                TempData["Error"] =
                    await respuesta.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarUsuarios(
            int? usuarioSeleccionado = null)
        {
            var usuarios = await _http
                .GetFromJsonAsync<List<Usuario>>(
                    "Usuarios"
                );

            usuarios ??= new List<Usuario>();

            var lista = usuarios
                .Select(x => new
                {
                    x.IdUsuario,

                    Descripcion =
                        $"Usuario #{x.IdUsuario} - {x.Correo}"
                })
                .ToList();

            ViewBag.Usuarios = new SelectList(
                lista,
                "IdUsuario",
                "Descripcion",
                usuarioSeleccionado
            );
        }
    }
}
