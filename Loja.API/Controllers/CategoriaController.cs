using Loja.WebAPICategorias.InputModels;
using Loja.Dominio.Entidades;
using Loja.Dominio.Interfaces;
using Loja.Dominio.Interfaces.IRepositorios;
using Microsoft.AspNetCore.Mvc;

namespace Loja.WebAPICategorias.Controllers
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
    [Route("api/v1/categorias")]
    public class CategoriaController : ControllerBase
    {
        /// <summary>
        /// Consulta todas as categorias.
        /// </summary>
        /// <returns>Uma lista de categorias</returns>
        /// <response code="200">Sucesso - Retorna uma lista de categorias.</response>
        [HttpGet]  //GET api/v1/categorias
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoriasAsync([FromServices] IRepositorioCategorias repositorioCategorias)
        {
            var listaCategorias = await repositorioCategorias.GetCategoriasAsync();

            return Ok(listaCategorias);
        }

        /// <summary>
        /// Consulta uma categoria específica.
        /// </summary>
        /// <response code="404">Not Found - Categoria não encontrada.</response>
        /// <response code="200">Sucesso - Retorna a categoria encontrada.</response>
        [HttpGet("{codigo:int}")] //GET api/v1/categorias/3
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Categoria))]
        public async Task<IActionResult> GetCategoriaByCodigoAsync([FromServices] IRepositorioCategorias repositorioCategorias,
                                                                   [FromRoute] int codigo)
        {
            var categoria = await repositorioCategorias.GetCategoriaByCodigoAsync(codigo);

            return categoria == null ? NotFound("Categoria não encontrada!") : Ok(categoria);
        }

        /// <summary>
        /// Cria uma nova categoria.
        /// </summary>
        /// <response code="400">Bad Request - Erro na requisição.</response>
        /// <response code="201">Sucesso - Retorna a categoria criada.</response>
        /// <response code="500">Server Error.</response>
        [HttpPost] //POST api/v1/categorias
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarCategoriaAsync([FromServices] IRepositorioCategorias repositorioCategorias,
                                                             [FromServices] IUnidadeDeTrabalho unidadeDeTrabalho,
                                                             [FromBody] CreateCategoriaInputModel categoriaInputModel)
        {
            var novaCategoria = new Categoria(categoriaInputModel.Titulo);
            
            try
            {
                //No caso de ORM(EF) esse codigo vai receber sempre zero, porem se trocar a fonte de dados será necessario.
                novaCategoria.Codigo = await repositorioCategorias.CriarCategoriaAsync(novaCategoria);
                await unidadeDeTrabalho.CommitAsync();

                return Created($"api/v1/categorias/{novaCategoria.Codigo}", novaCategoria);
            }
            catch (Exception ex)
            {
                await unidadeDeTrabalho.RollBackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma categoria específica.
        /// </summary>
        /// <response code="400">Bad Request - Erro na requisição.</response>
        /// <response code="404">Not Found - Categoria não encontrada.</response>
        /// <response code="200">Sucesso - Retorna a categoria alterada.</response>
        /// <response code="500">Server Error.</response>
        [HttpPut("{codigo:int}")] //PUT api/v1/categorias
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Categoria))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarCategoriaAsync([FromServices] IRepositorioCategorias repositorioCategorias,
                                                                 [FromServices] IUnidadeDeTrabalho unidadeDeTrabalho,
                                                                 [FromBody] UpdateCategoriaInputModel categoriaInputModel,
                                                                 [FromRoute] int codigo)
        {
            var categoria = await repositorioCategorias.GetCategoriaByCodigoAsync(codigo);

            if (categoria == null)
                return NotFound("Categoria não encontrada!");

            categoria.Titulo = categoriaInputModel.Titulo;

            try
            {
                repositorioCategorias.AtualizarCategoria(categoria);
                await unidadeDeTrabalho.CommitAsync();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                await unidadeDeTrabalho.RollBackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deleta uma categoria específica.
        /// </summary>
        /// <response code="404">Not Found - Categoria não encontrada.</response>
        /// <response code="200">Sucesso</response>
        /// <response code="500">Server Error.</response>
        [HttpDelete("{codigo:int}")] //DELETE api/v1/categorias/3
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarCategoriaAsync([FromServices] IRepositorioCategorias repositorioCategorias,
                                                               [FromServices] IUnidadeDeTrabalho unidadeDeTrabalho,
                                                               [FromRoute] int codigo)
        {
            var categoria = await repositorioCategorias.GetCategoriaByCodigoAsync(codigo);

            if (categoria == null)
                return NotFound("Categoria não encontrada!");

            try
            {
                repositorioCategorias.DeletarCategoria(categoria);
                await unidadeDeTrabalho.CommitAsync();

                return Ok("Categoria deletada com Sucesso!");
            }
            catch (Exception ex)
            {
                await unidadeDeTrabalho.RollBackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
