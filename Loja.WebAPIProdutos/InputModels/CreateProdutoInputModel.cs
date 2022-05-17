using System.ComponentModel.DataAnnotations;

namespace Loja.WebAPIProdutos.InputModels
{
    public class CreateProdutoInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }

        [Required]
        public decimal Preco { get; set; }

        //[Required]
        //public int Categoria { get; set; }
    }
}
