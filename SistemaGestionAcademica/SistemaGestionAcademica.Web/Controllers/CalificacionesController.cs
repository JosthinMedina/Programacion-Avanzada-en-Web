using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class CalificacionesController : Controller
    {
        private readonly HttpClient _http;

        public CalificacionesController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var calificaciones = await _http
                .GetFromJsonAsync<List<Calificacion>>("Calificaciones");

            var matriculas = await _http
                .GetFromJsonAsync<List<Matricula>>("Matriculas");

            var estudiantes = await _http
                .GetFromJsonAsync<List<Estudiante>>("Estudiantes");

            var cursos = await _http
                .GetFromJsonAsync<List<Curso>>("Cursos");

            ViewBag.Matriculas = matriculas ?? new List<Matricula>();
            ViewBag.Estudiantes = estudiantes ?? new List<Estudiante>();
            ViewBag.Cursos = cursos ?? new List<Curso>();

            return View(calificaciones ?? new List<Calificacion>());
        }

        public async Task<IActionResult> Registrar()
        {
            await CargarMatriculas();

            return View(new Calificacion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(
            Calificacion calificacion)
        {
            if (!ModelState.IsValid)
            {
                await CargarMatriculas(calificacion.IdMatricula);

                return View(calificacion);
            }

            var respuesta = await _http.PostAsJsonAsync(
                "Calificaciones",
                calificacion
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Calificación registrada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarMatriculas(calificacion.IdMatricula);

            return View(calificacion);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Calificaciones/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var calificacion = await respuesta.Content
                .ReadFromJsonAsync<Calificacion>();

            if (calificacion == null)
                return RedirectToAction(nameof(Index));

            await CargarMatriculas(calificacion.IdMatricula);

            return View(calificacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            Calificacion calificacion)
        {
            if (!ModelState.IsValid)
            {
                await CargarMatriculas(calificacion.IdMatricula);

                return View(calificacion);
            }

            var respuesta = await _http.PutAsJsonAsync(
                "Calificaciones",
                calificacion
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Calificación actualizada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarMatriculas(calificacion.IdMatricula);

            return View(calificacion);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _http.DeleteAsync(
                $"Calificaciones/{id}"
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Calificación eliminada correctamente.";
            }
            else
            {
                TempData["Error"] =
                    await respuesta.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarMatriculas(
            int? matriculaSeleccionada = null)
        {
            var matriculas = await _http
                .GetFromJsonAsync<List<Matricula>>("Matriculas");

            var estudiantes = await _http
                .GetFromJsonAsync<List<Estudiante>>("Estudiantes");

            var cursos = await _http
                .GetFromJsonAsync<List<Curso>>("Cursos");

            matriculas ??= new List<Matricula>();
            estudiantes ??= new List<Estudiante>();
            cursos ??= new List<Curso>();

            var lista = matriculas
                .Where(x => x.Estado)
                .Select(x =>
                {
                    var estudiante = estudiantes.FirstOrDefault(
                        e => e.IdEstudiante == x.IdEstudiante
                    );

                    var curso = cursos.FirstOrDefault(
                        c => c.IdCurso == x.IdCurso
                    );

                    var nombreEstudiante = estudiante == null
                        ? $"Estudiante #{x.IdEstudiante}"
                        : $"{estudiante.Nombre} " +
                          $"{estudiante.PrimerApellido} " +
                          $"{estudiante.SegundoApellido}";

                    var nombreCurso = curso == null
                        ? $"Curso #{x.IdCurso}"
                        : curso.NombreCurso;

                    return new
                    {
                        x.IdMatricula,
                        Descripcion =
                            $"Matrícula #{x.IdMatricula} - " +
                            $"{nombreEstudiante} - {nombreCurso}"
                    };
                })
                .ToList();

            ViewBag.Matriculas = new SelectList(
                lista,
                "IdMatricula",
                "Descripcion",
                matriculaSeleccionada
            );
        }
    }
}
