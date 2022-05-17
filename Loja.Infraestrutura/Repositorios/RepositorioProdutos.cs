using Loja.Dominio.Entidades;
using Loja.Dominio.Interfaces.IRepositorios;
using Loja.Infraestrutura.DBContexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Loja.Infraestrutura.Repositorios
{
    public class RepositorioProdutos : IRepositorioProdutos
    {
        private readonly DBContextLoja _DBContextLoja;

        public RepositorioProdutos(DBContextLoja DBContextLoja)
        {
            _DBContextLoja = DBContextLoja;
        }

        public async Task<int> CriarProdutoAsync(Produto novoProduto)
        {
            try
            {
                await _DBContextLoja.Produtos.AddAsync(novoProduto);
                return novoProduto.Codigo;//vai ser sempre 0 no caso do ORM. Mas como tem q ter um retorno pra dar suporte a outras fontes de dados...
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public bool AtualizarProduto(Produto produtoAlterado)
        {
            try
            {
                _DBContextLoja.Produtos.Update(produtoAlterado);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public bool DeletarProduto(Produto produto)
        {
            try
            {
                _DBContextLoja.Produtos.Remove(produto);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<List<Produto>> GetProdutosAsync()
        {   
            return await _DBContextLoja.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<Produto> GetProdutoByCodigoAsync(int codigo)
        {
            return await _DBContextLoja.Produtos.AsNoTracking().SingleOrDefaultAsync(p => p.Codigo == codigo);
        }

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        private bool disposedValue;
        // Instantiate a SafeHandle instance.
        //SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    // handle.Dispose();
                    // Free any other managed objects here.
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RepositorioProdutos()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: false);
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
