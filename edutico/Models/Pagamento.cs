namespace edutico.Models
{
    public class Pagamento
    {
        public int codPag { get; set; }
        public int qtdParcela { get; set; }
        public Pedido pedido { get; set; }
        public string codPix { get; set; }
        public Cartao cartao { get; set; }
        public int tipoPag { get; set; }
        public Boolean retorno { get; set; }
        public string mensagem { get; set; }
        public List<Cartao> cartoes { get; set; }
    }
}
