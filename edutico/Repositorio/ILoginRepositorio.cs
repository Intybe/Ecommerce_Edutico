using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{
    public interface ILoginRepositorio
    {
        Login Login(string usuario, string senha);

        void AlterarSenha(decimal codLogin, string Senha);

        decimal ObterCodLoginPorUsuario(string usuario);
    }

   
}
    