using edutico.Models;

namespace edutico.Repositorio
{
    public interface IProdutoRepositorio
    {
        string CadastrarProduto(Produto produto, List<IFormFile> imgs, List<string> habilidades);

        IEnumerable<Produto> ConsultarProdutoLancamento();

        Produto ConsultarDetalheProduto(decimal codProd);

        string CadastrarAvaliacao(int qtdEstrela, string comentario, int codLogin, decimal codProd);
    }
}
