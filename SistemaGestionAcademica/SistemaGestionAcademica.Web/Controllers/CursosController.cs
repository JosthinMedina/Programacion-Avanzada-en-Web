using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class CursosController : Controller
    {
        private readonly HttpClient _http;

        public CursosController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var cursos = await _http.GetFromJsonAsync<List<Curso>>("Cursos");

            var profesores = await _http.GetFromJsonAsync<List<Profesor>>(
                "Profesores"
            );

            ViewBag.Profesores = profesores ?? new List<Profesor>();

            return View(cursos ?? new List<Curso>());
        }

        public async Task<IActionResult> Registrar()
        {
            await CargarProfesores();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Curso curso)
        {
            if (!ModelState.IsValid)
            {
                await CargarProfesores(curso.IdProfesor);
                return View(curso);
            }

            var respuesta = await _http.PostAsJsonAsync(
                "Cursos",
                curso
            );

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();

            await CargarProfesores(curso.IdProfesor);

            return View(curso);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Cursos/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var curso = await respuesta.Content
                .ReadFromJsonAsync<Curso>();

            if (curso == null)
                return RedirectToAction(nameof(Index));

            await CargarProfesores(curso.IdProfesor);

            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Curso curso)
        {
            if (!ModelState.IsValid)
            {
                await CargarProfesores(curso.IdProfesor);
                return View(curso);
            }

            var respuesta = await _http.PutAsJsonAsync(
                "Cursos",
                curso
            );

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();

            await CargarProfesores(curso.IdProfesor);

            return View(curso);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            await _http.DeleteAsync($"Cursos/{id}");

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarProfesores(
            int? profesorSeleccionado = null)
        {
            var profesores = await _http
                .GetFromJsonAsync<List<Profesor>>("Profesores");

            var profesoresActivos = profesores?
                .Where(x => x.Estado)
                .Select(x => new
                {
                    x.IdProfesor,
                    NombreCompleto =
                        $"{x.Nombre} {x.PrimerApellido} {x.SegundoApellido}"
                })
                .ToList();

            ViewBag.Profesores = new SelectList(
                profesoresActivos ?? [],
                "IdProfesor",
                "NombreCompleto",
                profesorSeleccionado
            );
        }
    }
}
