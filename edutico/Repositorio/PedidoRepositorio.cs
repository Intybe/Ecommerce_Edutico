using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;


namespace edutico.Repositorio
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        public string CadastrarPedido(Pedido pedido)
        {
            // Cria a variável de conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável para armazenar a mensagem de retorno
            string mensagem = null;


            // 1. Cadastra o pedido na tabela tbPedido chamando a procedure spInsertTbPedido
            string sqlPedido = "Call spInsertTbPedido(@codLogin, @valorTotal);";

            MySqlCommand cmd = new MySqlCommand(sqlPedido, conexao);
            cmd.Parameters.AddWithValue("@codLogin", pedido.cliente.codLogin);
            cmd.Parameters.AddWithValue("@valorTotal", pedido.valorTotal);

            int nf = 0;

            // 2. Executa o comando e obtém o ID do pedido
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    // Lê o valor de NF como um inteiro
                    nf = reader.GetInt32("NF");
                }
            }

            // 3. Cadastra os itens do pedido na tabela tbItemPedido chamando a procedure spInsertTbItemPedido
            foreach (var item in pedido.itensPedido)
            {
                string sqlItem = "Call spInsertTbItemPedido(@nf, @codProd, @qtdItem, @valorProd);";
                cmd = new MySqlCommand(sqlItem, conexao);
                cmd.Parameters.AddWithValue("@nf", nf);
                cmd.Parameters.AddWithValue("@codProd", item.produto.codProd);
                cmd.Parameters.AddWithValue("@qtdItem", item.qtdItem);
                cmd.Parameters.AddWithValue("@valorProd", item.produto.valorUnit);

                // Executa o comando de inserção dos itens
                cmd.ExecuteNonQuery();
            }

            mensagem = nf.ToString();
            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            return mensagem;
        }
    }
}

