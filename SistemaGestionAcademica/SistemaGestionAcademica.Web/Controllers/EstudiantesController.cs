using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly HttpClient _http;

        public EstudiantesController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _http
                .GetFromJsonAsync<List<Estudiante>>("Estudiantes");

            return View(lista ?? new List<Estudiante>());
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(
            Estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                return View(estudiante);
            }

            var respuesta = await _http.PostAsJsonAsync(
                "Estudiantes",
                estudiante
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Estudiante registrado correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            return View(estudiante);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Estudiantes/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var estudiante = await respuesta.Content
                .ReadFromJsonAsync<Estudiante>();

            if (estudiante == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(estudiante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            Estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                return View(estudiante);
            }

            var respuesta = await _http.PutAsJsonAsync(
                "Estudiantes",
                estudiante
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Estudiante actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            return View(estudiante);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _http.DeleteAsync(
                $"Estudiantes/{id}"
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Estudiante eliminado correctamente.";
            }
            else
            {
                TempData["Error"] =
                    await respuesta.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
