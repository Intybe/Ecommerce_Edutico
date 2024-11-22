using edutico.Models;


namespace edutico.Repositorio
{
    public interface IProdutoRepositorio
    {
        string CadastrarProduto(Produto produto, List<IFormFile> imgs);

        IEnumerable<Produto> ConsultarProdutoLancamento();

        Produto ConsultarDetalheProduto(decimal codProd);

        string CadastrarAvaliacao(int qtdEstrela, string comentario, int codLogin, decimal codProd);

        IEnumerable<Produto> ConsultarProdutoPesquisa(string pesquisa);

        IEnumerable<Produto> ConsultarProdutoF();

        void AtualizarStatusProduto(decimal codProd, int statusProd);

        void DeletarAvaliacao(int codLogin, int codAvaliacao);

        Avaliacao ConsultarAvaliacaoCliente(decimal codProd, int codLogin);
    }
}