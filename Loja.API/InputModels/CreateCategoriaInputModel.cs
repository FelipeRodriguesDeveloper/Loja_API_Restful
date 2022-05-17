using System.ComponentModel.DataAnnotations;

namespace Loja.WebAPICategorias.InputModels
{
    public class CreateCategoriaInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }
    }
}
