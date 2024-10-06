namespace edutico.Models
{
    public class Pagamento
    {
        public int codPag { get; set; }
        public int qtdParcela { get; set; }
        public int NF { get; set; }
        public string codPix { get; set; }
        public Cartao cartao { get; set; }
    }
}
