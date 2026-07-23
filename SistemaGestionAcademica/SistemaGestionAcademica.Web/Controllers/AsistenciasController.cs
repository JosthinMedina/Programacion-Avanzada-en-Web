using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly HttpClient _http;

        public AsistenciasController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var asistencias = await _http
                .GetFromJsonAsync<List<Asistencia>>("Asistencias");

            var matriculas = await _http
                .GetFromJsonAsync<List<Matricula>>("Matriculas");

            var estudiantes = await _http
                .GetFromJsonAsync<List<Estudiante>>("Estudiantes");

            var cursos = await _http
                .GetFromJsonAsync<List<Curso>>("Cursos");

            ViewBag.Matriculas = matriculas ?? new List<Matricula>();
            ViewBag.Estudiantes = estudiantes ?? new List<Estudiante>();
            ViewBag.Cursos = cursos ?? new List<Curso>();

            return View(asistencias ?? new List<Asistencia>());
        }

        public async Task<IActionResult> Registrar()
        {
            await CargarMatriculas();

            return View(new Asistencia
            {
                Fecha = DateTime.Today,
                Estado = "Presente"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Asistencia asistencia)
        {
            if (!EstadoValido(asistencia.Estado))
            {
                ModelState.AddModelError(
                    nameof(asistencia.Estado),
                    "El estado debe ser Presente, Ausente o Justificada."
                );
            }

            if (!ModelState.IsValid)
            {
                await CargarMatriculas(asistencia.IdMatricula);

                return View(asistencia);
            }

            var respuesta = await _http.PostAsJsonAsync(
                "Asistencias",
                asistencia
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Asistencia registrada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarMatriculas(asistencia.IdMatricula);

            return View(asistencia);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Asistencias/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "La asistencia seleccionada no existe.";

                return RedirectToAction(nameof(Index));
            }

            var asistencia = await respuesta.Content
                .ReadFromJsonAsync<Asistencia>();

            if (asistencia == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await CargarMatriculas(asistencia.IdMatricula);

            return View(asistencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Asistencia asistencia)
        {
            if (!EstadoValido(asistencia.Estado))
            {
                ModelState.AddModelError(
                    nameof(asistencia.Estado),
                    "El estado debe ser Presente, Ausente o Justificada."
                );
            }

            if (!ModelState.IsValid)
            {
                await CargarMatriculas(asistencia.IdMatricula);

                return View(asistencia);
            }

            var respuesta = await _http.PutAsJsonAsync(
                "Asistencias",
                asistencia
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Asistencia actualizada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarMatriculas(asistencia.IdMatricula);

            return View(asistencia);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _http.DeleteAsync(
                $"Asistencias/{id}"
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Asistencia eliminada correctamente.";
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

        private static bool EstadoValido(string estado)
        {
            return estado == "Presente"
                || estado == "Ausente"
                || estado == "Justificada";
        }
    }
}
