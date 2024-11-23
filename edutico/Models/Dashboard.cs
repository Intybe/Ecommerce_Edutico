namespace edutico.Models
{
    public class Dashboard
    {
        public int pedidosPendentes { get; set; }
        public int qtdFavoritos { get; set; }
        public int qtdVendasAtual { get; set; }
        public int qtdVendasAnterior { get; set; }
        public int produtosEsgotados { get; set; }
        public List<Produto> maisVendidos { get; set; }
        public List<decimal> vendaDiaria { get; set; }
        public decimal totalVendaSemanal { get; set; }
        public List<decimal> vendaSemanal { get; set; }
        public decimal totalVendaMensal { get; set; }
        public List<decimal> vendaMensal { get; set; }
        public decimal totalVendaAnual { get; set; }
    }
}
