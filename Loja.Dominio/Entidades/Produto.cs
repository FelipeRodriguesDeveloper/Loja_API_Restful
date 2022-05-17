
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.Dominio.Entidades
{
    public class Produto
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public decimal Preco { get; set; }
        //public int CodigoCategoria { get; set; }
        //public Categoria Categoria { get; set; } //Propriedade de Navegação de Referencia
    }
}
