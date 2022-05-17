
namespace Loja.Dominio.Interfaces
{
    public interface IUnidadeDeTrabalho : IDisposable
    {
        public Task<bool> CommitAsync();

        public Task RollBackAsync();
    }
}
