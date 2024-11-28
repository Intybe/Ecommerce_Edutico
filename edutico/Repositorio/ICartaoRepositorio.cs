using edutico.Models;

namespace edutico.Repositorio
{
    public interface ICartaoRepositorio
    {
        string CadastrarCartao(decimal NumCartao, string NomeTitular, string DataVali, string Bandeira, int CodLogin);
        List<Cartao> ConsultarCartao(int CodLogin);
    }
}