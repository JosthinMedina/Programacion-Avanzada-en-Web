using Microsoft.AspNetCore.Mvc;
using SistemaGestionAcademica.Web.Models;
using System.Net.Http.Json;

namespace SistemaGestionAcademica.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UsuariosController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private HttpClient CrearCliente()
        {
            var cliente = _httpClientFactory.CreateClient();

            cliente.BaseAddress = new Uri(
                _configuration["ApiSettings:BaseUrl"]!
            );

            return cliente;
        }

        public async Task<IActionResult> Index()
        {
            var cliente = CrearCliente();

            var usuarios = await cliente.GetFromJsonAsync<List<Usuario>>(
                "Usuarios"
            );

            return View(usuarios ?? new List<Usuario>());
        }
    }
}
