
using Loja.Dominio.Interfaces;
using Loja.Infraestrutura.DBContexto;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Loja.Infraestrutura.UnidadeDeTrabalho
{
    public class UnidadeDeTrabalho : IUnidadeDeTrabalho
    {
        private readonly DBContextLoja _DBContextLoja;

        public UnidadeDeTrabalho(DBContextLoja DBContextLoja)
        {
            _DBContextLoja = DBContextLoja;
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                var sucesso = (await _DBContextLoja.SaveChangesAsync()) > 0;

                //Aqui poderia disparar eventos de Dominio etc... eventos que chamam alteracoes em outras tabelas etc...
                return sucesso;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public Task RollBackAsync()
        {
            //nao faz nada pois no EF nao precisa
            return Task.CompletedTask;
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
                    //handle.Dispose();
                    // Free any other managed objects here.
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~UnidadeDeTrabalho()
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
