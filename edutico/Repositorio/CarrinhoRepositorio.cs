using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{

    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        public Carrinho CadastrarProdutoCarrinho(int codLogin, decimal codProd, int qtdProd)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "spInsertTbCarrinho(@codLogin, @codProd, @qtdProd);";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codProd", codProd);
            cmd.Parameters.AddWithValue("@qtdProd", qtdProd);

            // Este código executa a procedure em caso onde não tenha retorno 

            cmd.ExecuteNonQuery();
            Carrinho carrinho = new Carrinho();
            return carrinho;

            // deconecta do banco de dados
            con.DesconectarBD();
        }
    }
}
