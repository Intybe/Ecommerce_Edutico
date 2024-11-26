
namespace edutico.Models
{
    public class ViewProdutoFavoritos
    {
        public List<Produto> produtos { get; set; }
        public List<decimal> favoritos { get; set; }
        public Produto produto { get; set; } = new Produto();

        public Avaliacao avaliacaoUnica { get; set; }

    }
}
