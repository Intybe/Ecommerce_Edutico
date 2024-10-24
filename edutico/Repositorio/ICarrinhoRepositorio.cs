using edutico.Models;

namespace edutico.Repositorio
{
    public interface ICarrinhoRepositorio
    {

        // Instanciando o método de consulta de itens do carrinho
        IEnumerable<Carrinho> ConsultarCarrinho(int codLogin);

        // Instanciando o método de cadastro dos produtos no carrinho
        public String CadastrarProdutoCarrinho(int codLogin, decimal codProd, int qtdProd);

        public String ExcluirItemCarrinho(int codLogin, decimal codProd);

        public String AtualizarQtdProdCarrinho(int codLogin, decimal codProd, int qtdProd);

    }
}

