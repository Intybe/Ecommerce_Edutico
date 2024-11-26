using edutico.Models;


namespace edutico.Repositorio
{
    public interface IProdutoRepositorio
    {
        // Método para Cadastrar o produto com mensagem de retorno
        string CadastrarProduto(Produto produto, List<IFormFile> imgs);

        // Método para consultar os produtos em laçamento que retorna vários objetos produto
        IEnumerable<Produto> ConsultarProdutoLancamento();

        // Método consultar detalhes do produto
        Produto ConsultarDetalheProduto(decimal codProd);

        // Método para cadastrar avaliação
        string CadastrarAvaliacao(int qtdEstrela, string comentario, int codLogin, decimal codProd);

        // Método para pesquisar os produtos
        IEnumerable<Produto> ConsultarProdutoPesquisa(string pesquisa);

        // Método para consultar todos os produtos
        IEnumerable<Produto> ConsultarProdutoF();

        // Método para ativar os desativar produto
        void AtualizarStatusProduto(decimal codProd, int statusProd);

        // Método para deletar avaliação
        string DeletarAvaliacao(int codLogin, int codAvaliacao, decimal codProd);

        // Método para consultar informações do Dashboard
        Dashboard ConsultarInfosEcommerce();

        // Método para consultar os produtos favoritados na visão do funcionário
        List<Produto> ConsultarProdutosFavoritos();

        // Método para consultar a avalição do cliente
        Avaliacao ConsultarAvaliacaoCliente(decimal codProd, int codLogin);

        Produto ConsultarDetalheProdutoF(decimal codProd);
    }
}