using edutico.Data;
using MySql.Data.MySqlClient;

namespace edutico.Models
{
    public class Produto
    {
        public decimal nf { get; set; }
        public string nomeProd { get; set; }
        public string descricao { get; set; }
        public int codClassificacao { get; set; }
        public int codCategoria { get; set; }
        public bool lancamento { get; set; }
        public decimal valorUnit { get; set; }
        public bool statusProd { get; set; }

        public void cadastrarProduto(decimal nf, string nomeProd, string descricao, int codClassificacao, int codCategoria, bool lancamento, decimal valorUnit, bool statusProd)
        {
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            string sql = "Call spInsertTbProduto(@nf, @nomeProd, @descricao, @codClassificacao, @codCategoria, @lacamento, @valorUnit, @statusProd)";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nf", nf);
            cmd.Parameters.AddWithValue("@nome", nomeProd);
            cmd.Parameters.AddWithValue("@descricao", descricao);
            cmd.Parameters.AddWithValue("@codClassificacao", codClassificacao); 
            cmd.Parameters.AddWithValue("@codCategoria", codCategoria);
            cmd.Parameters.AddWithValue("@lacamento", lancamento);
            cmd.Parameters.AddWithValue("@valorUnit", valorUnit);
            cmd.Parameters.AddWithValue("@statusProd", statusProd);
            cmd.ExecuteNonQuery();

            con.DesconectarBD();

        }
    }
}