using edutico.Models;

namespace edutico.Repositorio
{
    public interface IProdutoRepositorio
    {
        string CadastrarProduto(Produto produto, List<IFormFile> imgs, List<string> habilidades);

        IEnumerable<Produto> ConsultarProdutoLacamento();

        Produto ConsultarProduto(decimal codProd);
    }
}
