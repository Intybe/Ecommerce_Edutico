using edutico.Models;

namespace edutico.Repositorio
{
    public interface ICarrinhoRepositorio
    {
        Carrinho CadastrarProdutoCarrinho(int codLogin, decimal codProd, int qtdProd); 
    }
}

