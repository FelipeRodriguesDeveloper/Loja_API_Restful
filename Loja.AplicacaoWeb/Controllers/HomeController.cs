using Loja.AplicacaoWeb.Helper;
using Loja.AplicacaoWeb.Models;
using Loja.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Loja.AplicacaoWeb.Controllers
{
    public class HomeController : Controller
    {
        CategoriasAPI _categoriasAPI = new CategoriasAPI();

        public async Task<IActionResult> Index()
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

        [HttpGet("DetalharCategoria/{codigo}")]  
        public async Task<IActionResult> DetalharCategoria(int codigo)
        {
            Categoria categoria = new Categoria();

            HttpClient client = _categoriasAPI.Iniciar();
            HttpResponseMessage resposta = await client.GetAsync($"api/v1/categorias/{codigo}");

            if (resposta.IsSuccessStatusCode)
            {
                var resultado = resposta.Content.ReadAsStringAsync().Result;
                categoria = JsonConvert.DeserializeObject<Categoria>(resultado);
            }

            return View(categoria);
        }
        
        public async Task<IActionResult> CriarCategoria()
        {
            return View();  
        }

        [HttpPost] 
        public async Task<IActionResult> CriarCategoria(Categoria novaCategoria)
        {
            HttpClient client = _categoriasAPI.Iniciar();
            HttpResponseMessage resposta = await client.PostAsJsonAsync<Categoria>("api/v1/categorias", novaCategoria);

            if (resposta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(String.Empty,"Erro de servidor. Por favor, informe o administrador!");

            return View();
        }

        public async Task<IActionResult> AlterarCategoria(int codigo)
        {
            Categoria categoria = new Categoria();

            HttpClient client = _categoriasAPI.Iniciar();
            HttpResponseMessage resposta = await client.GetAsync($"api/v1/categorias/{codigo}");

            if (resposta.IsSuccessStatusCode)
            {
                var resultado = resposta.Content.ReadAsStringAsync().Result;
                categoria = JsonConvert.DeserializeObject<Categoria>(resultado);
            }

            return View(categoria);
        }

        [HttpPut]
        public async Task<IActionResult> AlterarCategoria(Categoria categoriaAlterada)
        {
            HttpClient client = _categoriasAPI.Iniciar();
            HttpResponseMessage resposta = await client.PutAsJsonAsync<Categoria>($"api/v1/categorias/{categoriaAlterada.Codigo}", categoriaAlterada);

            if (resposta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); //View(categoriaAlterada);
            }

            ModelState.AddModelError(String.Empty,"Erro de servidor. Por favor, informe o administrador!");

            return View();
        }

        public async Task<IActionResult> DeletarCategoria()
        {
            return View();
        }
     
        [HttpGet("DeletarCategoria/{codigo}")]
        public async Task<IActionResult> DeletarCategoria(int codigo)
        {
            HttpClient client = _categoriasAPI.Iniciar();
            HttpResponseMessage resposta = await client.DeleteAsync($"api/v1/categorias/{codigo}");

            if (resposta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(String.Empty, "Erro de servidor. Por favor, informe o administrador!");

            return View();
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