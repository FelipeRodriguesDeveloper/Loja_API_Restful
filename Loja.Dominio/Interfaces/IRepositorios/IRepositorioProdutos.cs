using Loja.Dominio.Entidades;

namespace Loja.Dominio.Interfaces.IRepositorios
{
    public interface IRepositorioProdutos : IDisposable
    {
        public Task<int> CriarProdutoAsync(Produto novoProduto);

        public bool AtualizarProduto(Produto produtoAlterado);

        public bool DeletarProduto(Produto produto);

        public Task<List<Produto>> GetProdutosAsync();

        public Task<Produto> GetProdutoByCodigoAsync(int codigo);
    }
}
