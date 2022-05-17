using Loja.Dominio.Entidades;
using Loja.Dominio.Enums;
using Loja.Dominio.Interfaces.IRepositorios;
using Loja.Infraestrutura.DBContexto;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infraestrutura.Repositorios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly DBContextLoja _DBContextLoja;

        public RepositorioUsuarios(DBContextLoja DBContextLoja)
        {
            _DBContextLoja = DBContextLoja;
        }

        public async Task<String> CriarUsuariosAsync(ApplicationUser novoUsuario)
        {
            try
            {
                await _DBContextLoja.ApplicationUsers.AddAsync(novoUsuario);
                return novoUsuario.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public bool AtualizarUsuario(ApplicationUser usuarioAlterado)
        {
            try
            {
                _DBContextLoja.ApplicationUsers.Update(usuarioAlterado);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public bool DeletarUsuario(ApplicationUser usuario)
        {
            try
            {
                _DBContextLoja.ApplicationUsers.Remove(usuario);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<List<ApplicationUser>> GetUsuariosAsync()
        {
            return await _DBContextLoja.ApplicationUsers.AsNoTracking().ToListAsync();
        }

        public async Task<ApplicationUser> GetUsuarioByCodigoAsync(string id)
        {
            return await _DBContextLoja.ApplicationUsers.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> AdicionarUsuario(string email, string senha, int idade, string celular)
        {
            try
            {
                using (_DBContextLoja)
                {
                    await _DBContextLoja.ApplicationUsers.AddAsync(new ApplicationUser
                    {
                        Email = email,
                        PasswordHash = senha,
                        Idade = idade,
                        Celular = celular,
                        Tipo = TipoUsuario.Comum
                    });

                    await _DBContextLoja.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ExisteUsuario(string email, string senha)
        {
            try
            {
                using (_DBContextLoja)
                {
                    return await _DBContextLoja.ApplicationUsers.Where(u => u.Email.Equals(email) && u.PasswordHash.Equals(senha))
                        .AsNoTracking()
                        .AnyAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }
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
        ~RepositorioUsuarios()
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
