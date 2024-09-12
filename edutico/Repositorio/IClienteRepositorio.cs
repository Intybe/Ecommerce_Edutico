namespace edutico.Repositorio
{
    public interface IClienteRepositorio
    {
        string CadastrarCliente(decimal cpf, string nome, string sobrenome, string telefone, string email, string logradouro, string bairro, string cidade, string uf, string cep, int numEnd, string compEnd, string senha);
    }

}
