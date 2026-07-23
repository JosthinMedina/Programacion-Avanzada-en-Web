using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class MatriculasController : Controller
    {
        private readonly HttpClient _http;

        public MatriculasController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var matriculas = await _http
                .GetFromJsonAsync<List<Matricula>>("Matriculas");

            var estudiantes = await _http
                .GetFromJsonAsync<List<Estudiante>>("Estudiantes");

            var cursos = await _http
                .GetFromJsonAsync<List<Curso>>("Cursos");

            ViewBag.Estudiantes = estudiantes ?? new List<Estudiante>();
            ViewBag.Cursos = cursos ?? new List<Curso>();

            return View(matriculas ?? new List<Matricula>());
        }

        public async Task<IActionResult> Registrar()
        {
            await CargarEstudiantes();
            await CargarCursos();

            return View(new Matricula
            {
                FechaMatricula = DateTime.Now,
                Estado = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Matricula matricula)
        {
            if (!ModelState.IsValid)
            {
                await CargarEstudiantes(matricula.IdEstudiante);
                await CargarCursos(matricula.IdCurso);

                return View(matricula);
            }

            var respuesta = await _http.PostAsJsonAsync(
                "Matriculas",
                matricula
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Matrícula registrada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();

            await CargarEstudiantes(matricula.IdEstudiante);
            await CargarCursos(matricula.IdCurso);

            return View(matricula);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Matriculas/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var matricula = await respuesta.Content
                .ReadFromJsonAsync<Matricula>();

            if (matricula == null)
                return RedirectToAction(nameof(Index));

            await CargarEstudiantes(matricula.IdEstudiante);
            await CargarCursos(matricula.IdCurso);

            return View(matricula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Matricula matricula)
        {
            if (!ModelState.IsValid)
            {
                await CargarEstudiantes(matricula.IdEstudiante);
                await CargarCursos(matricula.IdCurso);

                return View(matricula);
            }

            var respuesta = await _http.PutAsJsonAsync(
                "Matriculas",
                matricula
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Matrícula actualizada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = await respuesta.Content.ReadAsStringAsync();

            await CargarEstudiantes(matricula.IdEstudiante);
            await CargarCursos(matricula.IdCurso);

            return View(matricula);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _http.DeleteAsync(
                $"Matriculas/{id}"
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Matrícula desactivada correctamente.";
            }
            else
            {
                TempData["Error"] =
                    await respuesta.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarEstudiantes(
            int? estudianteSeleccionado = null)
        {
            var estudiantes = await _http
                .GetFromJsonAsync<List<Estudiante>>("Estudiantes");

            var lista = estudiantes?
                .Where(x => x.Estado)
                .Select(x => new
                {
                    x.IdEstudiante,
                    NombreCompleto =
                        $"{x.Nombre} {x.PrimerApellido} {x.SegundoApellido}"
                })
                .ToList();

            ViewBag.Estudiantes = new SelectList(
                lista ?? [],
                "IdEstudiante",
                "NombreCompleto",
                estudianteSeleccionado
            );
        }

        private async Task CargarCursos(
            int? cursoSeleccionado = null)
        {
            var cursos = await _http
                .GetFromJsonAsync<List<Curso>>("Cursos");

            var lista = cursos?
                .Where(x => x.Estado)
                .Select(x => new
                {
                    x.IdCurso,
                    x.NombreCurso
                })
                .ToList();

            ViewBag.Cursos = new SelectList(
                lista ?? [],
                "IdCurso",
                "NombreCurso",
                cursoSeleccionado
            );
        }
    }
}
