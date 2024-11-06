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
    }
}