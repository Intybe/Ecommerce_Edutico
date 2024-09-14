using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace edutico.Models
{
    public class Produto
    {
        public decimal codProd { get; set; }
        public string nomeProd { get; set; }
        public string descricao { get; set; }
        public string classificacao { get; set; }
        public string  categoria { get; set; }
        public bool lancamento { get; set; }
        public decimal valorUnit { get; set; }
        public int estoque { get; set; }
        public bool statusProd { get; set; }

        public List<Imagem> imgs { get; set; }

        // Construtor para inicializar a lista de imagens
        public Produto()
        {
            imgs = new List<Imagem>(); // Inicializa a lista vazia
        }
    }
}