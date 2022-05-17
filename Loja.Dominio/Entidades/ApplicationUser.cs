using Loja.Dominio.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.Dominio.Entidades
{
    //Classe para customizar o Identity da Microsoft...
    public class ApplicationUser : IdentityUser
    {
        [Column("usu_idade")]
        public int Idade { get; set; }

        [Column("usu_celular")]
        public string? Celular { get; set; }

        [Column("usu_tipo")]
        public TipoUsuario Tipo { get; set; }
    }
}
