using Loja.AplicacaoWeb.Helper;
using Loja.AplicacaoWeb2.Models;
using Loja.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Loja.AplicacaoWeb2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CategoriasAPI _categoriasAPI = new CategoriasAPI();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Index2()
        {
            List<Categoria> categorias = new List<Categoria>();

            HttpClient client = _categoriasAPI.Iniciar();
            HttpResponseMessage resposta = await client.GetAsync("api/v1/categorias");

            if (resposta.IsSuccessStatusCode)
            {
                var resultado = resposta.Content.ReadAsStringAsync().Result;
                categorias = JsonConvert.DeserializeObject<List<Categoria>>(resultado);
            }

            return View(categorias);
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