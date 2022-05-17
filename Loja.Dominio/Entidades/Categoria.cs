
namespace Loja.Dominio.Entidades
{
    public class Categoria
    {
        public Categoria(string titulo)
        {
            Titulo = titulo;
        }

        public Categoria()
        {}

        public int Codigo { get; set; }
        public string Titulo { get; set; } 

        //public ICollection<Produto> ListaProdutos { get; set; } //Propriedade de navegação de coleção
    }
}
