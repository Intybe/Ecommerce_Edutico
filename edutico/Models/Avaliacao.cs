namespace edutico.Models
{
    public class Avaliacao
    {
        public int codAvaliacao { get; set; }
        public int qtdEstrela { get; set; }
        public string comentario { get; set; }
        public Cliente cliente { get; set; }
        public decimal codProd { get; set; }
        
    }
}
