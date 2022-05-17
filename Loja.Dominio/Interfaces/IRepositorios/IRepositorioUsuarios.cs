
namespace Loja.Dominio.Interfaces.IRepositorios
{
    public interface IRepositorioUsuarios //Para caso queira personalizar e inserir campos diferentes na tabela que o identity gera
    {
        Task<bool> AdicionarUsuario(string email, string senha, int idade, string celular);
        Task<bool> ExisteUsuario(string email, string senha);
    }
}
