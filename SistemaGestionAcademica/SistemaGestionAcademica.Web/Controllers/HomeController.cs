using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.Web.Models;
using System.Diagnostics;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public HomeController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario model)
        {
            using var client = _http.CreateClient();
            var urlApi = _config["Valores:UrlApi"] + "Login/login";

            var response = await client.PostAsJsonAsync(urlApi, model);

            if (response.IsSuccessStatusCode)
            {
                var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
                return RedirectToAction("Dashboard", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ViewBag.Error = "Credenciales incorrectas o usuario inactivo";
                return View(model);
            }
            else
            {
                ViewBag.Error = "Error al conectar con el servidor";
                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
