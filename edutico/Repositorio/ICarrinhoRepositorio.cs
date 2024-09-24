using edutico.Models;

namespace edutico.Repositorio
{
    public interface ICarrinhoRepositorio
    {
        String CadastrarProdutoCarrinho(int codLogin, decimal codProd, int qtdProd);
    }
}

