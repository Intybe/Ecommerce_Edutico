using edutico.Models;

namespace edutico.Repositorio
{
    public interface IClienteRepositorio
    {
        string CadastrarCliente(decimal cpf, string nome, string sobrenome, string telefone, string email, string logradouro, string bairro, string cidade, string uf, string cep, int numEnd, string compEnd, string senha);

        Cliente ConsultarCliente(int codLogin);

        string AtualizarClienteConta(int codLogin, string email, string senha);

        string AtualizarClienteEndereco(int codLogin, string logradouro, string bairro, string cep, string cidade, string uf, int numEnd, string compEnd);

        string AtualizarClienteDados(int codLogin, decimal cpf, string nome, string sobrenome, string telefone);
    }

}
