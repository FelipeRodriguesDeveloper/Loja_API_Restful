using Microsoft.AspNetCore.Mvc;

namespace Loja.AplicacaoWeb.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
