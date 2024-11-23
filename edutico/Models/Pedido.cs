namespace edutico.Models
{
    public class Pedido
    {
        public int NF { get; set; }
        public DateTime data { get; set; }
        public Cliente cliente { get; set; }
        public int statusPedido { get; set; }
        public decimal valorTotal { get; set; }
        public List<ItemPedido> itensPedido { get; set; }


        // Construtor Genérico
        public Pedido() { }


        // Construtor para criar prévia dos pedidos 
        public Pedido(int NF, DateTime dataPedido, int codLogin, int statusPedido, decimal valorTotal, string itensPedido)
        {
            this.NF = NF;
            this.data = dataPedido;
            this.cliente = new Cliente { codLogin = codLogin };
            this.statusPedido = statusPedido;
            this.valorTotal = valorTotal;
            this.itensPedido = ParseItemPedido(itensPedido);
        }


        // Método para converter a string em obejtos ItemPedido
        public List<ItemPedido> ParseItemPedido(string itensPedidoBD)
        {
            // Cria uma lista de obejeto do tipo ItemPedido
            List<ItemPedido> itensPedido = new List<ItemPedido>();

            // Verifica se há algum item do pedido na string
            if (!string.IsNullOrEmpty(itensPedidoBD))
            {
                // Irá criar um objeto habilidade e adicioná-lo na lista para cada linha
                foreach (var campoPedido in (itensPedidoBD.Split(" | ")))
                {
                    // Divide e coloca a habilidade e seu código em um vetor
                    var itemPedido = campoPedido.Split(" -- ");

                    // Cria e adiciona um objeto habilidade na lista
                    itensPedido.Add(new ItemPedido()
                    {
                        // O código do item fica no índice zero
                        codItem = Convert.ToInt32(itemPedido[0]),

                        qtdItem = Convert.ToInt32(itemPedido[3]),

                        produto = new Produto()
                        {
                            nomeProd = itemPedido[1].ToString(),
                            codProd = Convert.ToDecimal(itemPedido[2]),

                            img = new Imagem()
                            {
                                nomeImg = itemPedido[5],
                                enderecoImg = itemPedido[6]
                            }
                        },

                        valorItem = Convert.ToDecimal(itemPedido[4])
                    });
                }
                return itensPedido; // Encerra o método e retorna os dados
            }
            return null;
        }
    }
}
