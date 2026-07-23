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
            var lista = await _http.GetFromJsonAsync<List<Estudiante>>("api/Estudiantes");
            return View(lista);
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return View(estudiante);

            var respuesta = await _http.PostAsJsonAsync("api/Estudiantes", estudiante);

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();
            return View(estudiante);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var estudiante = await _http.GetFromJsonAsync<Estudiante>($"api/Estudiantes/{id}");

            if (estudiante == null)
                return RedirectToAction(nameof(Index));

            return View(estudiante);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return View(estudiante);

            var respuesta = await _http.PutAsJsonAsync("api/Estudiantes", estudiante);

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();
            return View(estudiante);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            await _http.DeleteAsync($"api/Estudiantes/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
