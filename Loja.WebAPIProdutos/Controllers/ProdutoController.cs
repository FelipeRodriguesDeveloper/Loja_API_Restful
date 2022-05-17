using Loja.Dominio.Entidades;
using Loja.Dominio.Interfaces;
using Loja.Dominio.Interfaces.IRepositorios;
using Loja.WebAPIProdutos.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace Loja.WebAPIProdutos.Controllers
{
    /*
(Esta API segue em fase de desenvolvimento...)
Ainda faltam middlewares, autenticação/autorização etc...

Esta API Restful foi desenvolvida com:

- ASP.NET Core 6.0.4
- Entity Framework Core 6.0.5
- SQL Server 
- Fluent API + Data Annotation 
- Swagger + OpenAPI
- Padrões Repository e Unit of Work

Desenvolvedor: Felipe Rodrigues

GitHub:https://github.com/FelipeRodriguesDeveloper

Linkedin:https://br.linkedin.com/in/felipe-rodrigues-programador

Email: feliperodriguesdeveloper@hotmail.com
*/

    [ApiController]
    [Route("api/v1/produtos")]
    public class ProdutoController : ControllerBase
    {
        /// <summary>
        /// Consulta todos os produtos.
        /// </summary>
        /// <returns>Uma lista de produtos</returns>
        /// <response code="200">Sucesso - Retorna uma lista de produtos.</response>
        [HttpGet]  //GET api/v1/produtos
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProdutosAsync([FromServices] IRepositorioProdutos repositorioProdutos)
        {
            var listaProdutos = await repositorioProdutos.GetProdutosAsync();

            return Ok(listaProdutos);
        }

        /// <summary>
        /// Consulta um produto específico.
        /// </summary>
        /// <response code="404">Not Found - Produto não encontrado.</response>
        /// <response code="200">Sucesso - Retorna o produto encontrado.</response>
        [HttpGet("{codigo:int}")] //GET api/v1/produtos/3
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Produto))]
        public async Task<IActionResult> GetProdutoByCodigoAsync([FromServices] IRepositorioProdutos repositorioProdutos,
                                                           [FromRoute] int codigo)
        {
            var produto = await repositorioProdutos.GetProdutoByCodigoAsync(codigo);

            return produto == null ? NotFound("Produto não encontrado!") : Ok(produto);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <response code="400">Bad Request - Erro na requisição.</response>
        /// <response code="201">Sucesso - Retorna o produto criado.</response>
        /// <response code="500">Server Error.</response>
        [HttpPost] //POST api/v1/produtos
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarProdutoAsync([FromServices] IRepositorioProdutos repositorioProdutos,
                                                           [FromServices] IUnidadeDeTrabalho unidadeDeTrabalho,
                                                           [FromBody] CreateProdutoInputModel produtoInputModel)
        {
            var novoProduto = new Produto();
                novoProduto.Titulo = produtoInputModel.Titulo;
                novoProduto.Preco = produtoInputModel.Preco;

            try
            {
                //No caso de ORM(EF) esse codigo vai receber sempre zero, porem se trocar a fonte de dados será necessario.
                novoProduto.Codigo = await repositorioProdutos.CriarProdutoAsync(novoProduto);
                await unidadeDeTrabalho.CommitAsync();

                return Created($"api/v1/produtos/{novoProduto.Codigo}", novoProduto);
            }
            catch (Exception ex)
            {
                await unidadeDeTrabalho.RollBackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um produto específico.
        /// </summary>
        /// <response code="400">Bad Request - Erro na requisição.</response>
        /// <response code="404">Not Found - Produto não encontrado.</response>
        /// <response code="200">Sucesso - Retorna o produto alterado.</response>
        /// <response code="500">Server Error.</response>
        [HttpPut("{codigo:int}")] //PUT api/v1/produtos
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Produto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarProdutoAsync([FromServices] IRepositorioProdutos repositorioProdutos,
                                                                [FromServices] IUnidadeDeTrabalho unidadeDeTrabalho,
                                                                [FromBody] UpdateProdutoInputModel produtoInputModel,
                                                                [FromRoute] int codigo)
        {
            var produto = await repositorioProdutos.GetProdutoByCodigoAsync(codigo);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            produto.Titulo = produtoInputModel.Titulo;
            produto.Preco = produtoInputModel.Preco;

            try
            {
                repositorioProdutos.AtualizarProduto(produto);
                await unidadeDeTrabalho.CommitAsync();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                await unidadeDeTrabalho.RollBackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deleta um produto específico.
        /// </summary>
        /// <response code="404">Not Found - Produto não encontrado.</response>
        /// <response code="200">Sucesso</response>
        /// <response code="500">Server Error.</response>
        [HttpDelete("{codigo:int}")] //DELETE api/v1/produtos/3
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarProdutoAsync([FromServices] IRepositorioProdutos repositorioProdutos,
                                                               [FromServices] IUnidadeDeTrabalho unidadeDeTrabalho,
                                                               [FromRoute] int codigo)
        {
            var produto = await repositorioProdutos.GetProdutoByCodigoAsync(codigo);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            try
            {
                repositorioProdutos.DeletarProduto(produto);
                await unidadeDeTrabalho.CommitAsync();

                return Ok("Produto deletado com Sucesso!");
            }
            catch (Exception ex)
            {
                await unidadeDeTrabalho.RollBackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
