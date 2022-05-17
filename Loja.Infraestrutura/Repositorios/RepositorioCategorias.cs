using Loja.Dominio.Entidades;
using Loja.Dominio.Interfaces.IRepositorios;
using Loja.Infraestrutura.DBContexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Loja.Infraestrutura.Repositorios
{
    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly DBContextLoja _DBContextLoja;

        public RepositorioCategorias(DBContextLoja DBContextLoja)
        {
            _DBContextLoja = DBContextLoja;
        }

        public async Task<int> CriarCategoriaAsync(Categoria novaCategoria)
        {
            try
            {
                await _DBContextLoja.Categorias.AddAsync(novaCategoria);
                return novaCategoria.Codigo;//vai ser sempre 0 no caso do ORM. Mas como tem q ter um retorno pra dar suporte a outras fontes de dados...
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public bool AtualizarCategoria(Categoria categoriaAlterada)
        {
            try
            {
                _DBContextLoja.Categorias.Update(categoriaAlterada);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public bool DeletarCategoria(Categoria categoria)
        {
            try
            {
                _DBContextLoja.Categorias.Remove(categoria);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            return await _DBContextLoja.Categorias.AsNoTracking().ToListAsync();
        }

        public async Task<Categoria> GetCategoriaByCodigoAsync(int codigo)
        {
            return await _DBContextLoja.Categorias.AsNoTracking().SingleOrDefaultAsync(c => c.Codigo == codigo);
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
        ~RepositorioCategorias()
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
