using Loja.Dominio.Entidades;
using Loja.Dominio.Enums;
using Loja.WebAPILogins.Models;
using Loja.WebAPILogins.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Loja.WebAPILogins.Controllers
{
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly SignInManager<ApplicationUser> _signInManager; 

        public UsuarioController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //******************* USANDO IDENTITY: ****************************
        [AllowAnonymous]
        [HttpPost("/api/tokenidentity")]  
        public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
            {
                return Unauthorized();//401
            }

            var existeUsuario = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, lockoutOnFailure: false);

            if (existeUsuario.Succeeded)
            {
                //Mesmas informações colocadas na classe de configuracao Program.cs
                var token = new TokenJWTBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Empresa - Felipe Rodrigues Developer")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("UsuarioAPINumero", "1")
                .AddExpiry(5) //5 minutos... ideal é colocar 1 hora ou mais p usuario conseguir navegar sem expirar o token e ele ter q fazer login novamente.
                .Builder();

                return Ok(token.value);//200
            }
            else
            {
                return Unauthorized();//401
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/usuarioIdentity")]
        public async Task<IActionResult> AdicionarUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
            {
                return BadRequest("Dados Inválidos!");
            }

            var user = new ApplicationUser //ApplicationUser p/ representar nosso usuario customizado
            {
                UserName = login.Email,
                Email = login.Email,
                Idade = login.Idade,
                Celular = login.Celular,
                Tipo = TipoUsuario.Comum //vamos criar comuns, pois teriamos q ter uma pagina/outro metodo pra criar Administrador
            };

            var resultado = await _userManager.CreateAsync(user, login.Senha);

            if (resultado.Errors.Any())
            {
                return Ok(resultado.Errors);// OK??
            }

            //SÓ A CRIAÇÃO DE USUARIO NAO É SUFICIENTE! TEM QUE HAVER A CONFIRMAÇÃO, senao não usuario nao consegue acessar.
            // Geração de Confirmação caso precise
            //Geralmente qnd se criar um usuario num portal ele gera um email de confirmação p/ vc confirmar q é vc realmente q está criando aquele usuario
            // se vc nao gerar o email de confirmacao o usuario nao terá acesso ao portal.
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user); //Gera o código de confirmação, o usuario teria q enviar de volta esse codigo p/ confirmar
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //Se tivessemos uma Tela normal... enviariamos o email p/ usuario, ele clicaria num botao e seria chamada uma API e iria passar esse código de confirmação...
            //...mas nao temos tela entao ja vamos confirmar/simular o retorno do código de confirmacao:

            // simulando retorno email p/ confirmar usuario
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com Sucesso");
            else
                return Ok("Erro ao Confirmar o usuário");
        }
    }
}
