using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class EventosController : Controller
    {
        private readonly HttpClient _http;

        public EventosController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var eventos = await _http
                .GetFromJsonAsync<List<Evento>>("Eventos");

            var cursos = await _http
                .GetFromJsonAsync<List<Curso>>("Cursos");

            ViewBag.Cursos = cursos ?? new List<Curso>();

            return View(eventos ?? new List<Evento>());
        }

        public async Task<IActionResult> Registrar()
        {
            await CargarCursos();

            return View(new Evento
            {
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddHours(1),
                Estado = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Evento evento)
        {
            ValidarFechas(evento);

            if (!ModelState.IsValid)
            {
                await CargarCursos(evento.IdCurso);

                return View(evento);
            }

            var respuesta = await _http.PostAsJsonAsync(
                "Eventos",
                evento
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Evento registrado correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarCursos(evento.IdCurso);

            return View(evento);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var respuesta = await _http.GetAsync(
                $"Eventos/{id}"
            );

            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "El evento seleccionado no existe.";

                return RedirectToAction(nameof(Index));
            }

            var evento = await respuesta.Content
                .ReadFromJsonAsync<Evento>();

            if (evento == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await CargarCursos(evento.IdCurso);

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Evento evento)
        {
            ValidarFechas(evento);

            if (!ModelState.IsValid)
            {
                await CargarCursos(evento.IdCurso);

                return View(evento);
            }

            var respuesta = await _http.PutAsJsonAsync(
                "Eventos",
                evento
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Evento actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                await respuesta.Content.ReadAsStringAsync();

            await CargarCursos(evento.IdCurso);

            return View(evento);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _http.DeleteAsync(
                $"Eventos/{id}"
            );

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Evento eliminado correctamente.";
            }
            else
            {
                TempData["Error"] =
                    await respuesta.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
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

        private void ValidarFechas(Evento evento)
        {
            if (evento.FechaFin <= evento.FechaInicio)
            {
                ModelState.AddModelError(
                    nameof(evento.FechaFin),
                    "La fecha de finalización debe ser posterior a la fecha de inicio."
                );
            }
        }
    }
}
