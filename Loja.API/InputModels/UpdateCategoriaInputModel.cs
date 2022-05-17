using System.ComponentModel.DataAnnotations;

namespace Loja.WebAPICategorias.InputModels
{
    public class UpdateCategoriaInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }
    }
}
