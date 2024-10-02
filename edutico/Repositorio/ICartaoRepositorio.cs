using edutico.Models;

namespace edutico.Repositorio
{
    public interface ICartaoRepositorio
    {
        String CadastrarCartao(decimal NumCartao, string NomeTitular, int DataVali, int Bandeira, int CodLogin);

    }
}