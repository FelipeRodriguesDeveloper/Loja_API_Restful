using Loja.Dominio.Entidades;

namespace Loja.Dominio.Interfaces.IRepositorios
{
    public interface IRepositorioCategorias : IDisposable
    {
        public Task<int> CriarCategoriaAsync(Categoria novaCategoria);

        public bool AtualizarCategoria(Categoria categoriaAlterada);

        public bool DeletarCategoria(Categoria categoria);

        public Task<List<Categoria>> GetCategoriasAsync();

        public Task<Categoria> GetCategoriaByCodigoAsync(int codigo);
    }
}
