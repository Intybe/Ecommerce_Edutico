using edutico.Data;
using edutico.Models;
using Edutico.Models;
using MySql.Data.MySqlClient;


namespace edutico.Repositorio
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        public string CadastrarPedido(Pedido pedido, List<ItemPedido> itensPedido)
        {
            // Cria a variável de conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável para armazenar a mensagem de retorno
            string mensagem = null;

            try
            {
                // 1. Cadastra o pedido na tabela tbPedido chamando a procedure spInsertTbPedido
                string sqlPedido = "Call spInsertTbPedido(@codLogin);";

                MySqlCommand cmd = new MySqlCommand(sqlPedido, conexao);
                cmd.Parameters.AddWithValue("@codLogin", pedido.codLogin);

                // Executa o comando de inserção do pedido
                cmd.ExecuteNonQuery();

                // 2. Cadastra os itens do pedido na tabela tbItemPedido chamando a procedure spInsertTbItemPedido
                foreach (var item in pedido.itensPedido)
                {
                    string sqlItem = "Call spInsertTbItemPedido(@codLogin, @codProd, @qtdItem);";

                    cmd = new MySqlCommand(sqlItem, conexao);
                    cmd.Parameters.AddWithValue("@codLogin", pedido.codLogin);
                    cmd.Parameters.AddWithValue("@codProd", item.codProd);
                    cmd.Parameters.AddWithValue("@qtdItem", item.qtdItem);

                    // Executa o comando de inserção dos itens
                    cmd.ExecuteNonQuery();
                }

                mensagem = "Pedido cadastrado com sucesso!";
            }
            catch (Exception ex)
            {
                // Em caso de erro, captura a mensagem de exceção
                mensagem = "Erro ao cadastrar o pedido: " + ex.Message;
            }
            finally
            {
                // Fecha a conexão com o banco de dados
                con.DesconectarBD();
            }

            return mensagem;
        }
    }
}

