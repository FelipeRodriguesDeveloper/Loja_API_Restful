namespace Loja.AplicacaoWeb.Helper
{
    public class CategoriasAPI
    {
        public HttpClient Iniciar()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7281/");

            return client;
        }
    }
}
