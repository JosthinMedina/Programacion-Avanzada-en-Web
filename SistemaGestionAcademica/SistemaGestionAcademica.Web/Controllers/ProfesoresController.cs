using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class ProfesoresController : Controller
    {
        private readonly HttpClient _http;

        public ProfesoresController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _http.GetFromJsonAsync<List<Profesor>>(
                "Profesores"
            );

            return View(lista ?? new List<Profesor>());
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Profesor profesor)
        {
            if (!ModelState.IsValid)
                return View(profesor);

            var respuesta = await _http.PostAsJsonAsync(
                "Profesores",
                profesor
            );

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();

            return View(profesor);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Profesores/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var profesor = await respuesta.Content
                .ReadFromJsonAsync<Profesor>();

            if (profesor == null)
                return RedirectToAction(nameof(Index));

            return View(profesor);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Profesor profesor)
        {
            if (!ModelState.IsValid)
                return View(profesor);

            var respuesta = await _http.PutAsJsonAsync(
                "Profesores",
                profesor
            );

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();

            return View(profesor);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            await _http.DeleteAsync($"Profesores/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}
