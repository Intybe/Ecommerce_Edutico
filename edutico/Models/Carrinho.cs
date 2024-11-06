namespace edutico.Models
{
    public class Carrinho
    {
        public int codLogin { get; set; }
        public Produto produto { get; set; }
        public int qtdProd { get; set; }


        // Construtor genérico
        public Carrinho() { }


        // Construtor de itens do Carrinho
        public Carrinho(int codLogin, decimal codProd, string nomeProd, decimal valorUnit, string imgsConcatenadas, int qtdProd)
        {
            this.codLogin = codLogin;
            this.qtdProd = qtdProd;
            this.produto = new Produto(codProd, nomeProd, valorUnit, imgsConcatenadas);
        }
    }
}
