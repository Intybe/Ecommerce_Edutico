using edutico.Data;
using edutico.Models;
using Microsoft.AspNetCore.Mvc;
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

        public List<Pedido> ConsultarPedidos(int codLogin)
        {
            // Cria a variável de conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Select tbPedido.*, codItem, codprod, qtdItem, valorItem  from tbPedido Join tbItemPedido On tbPedido.NF = tbItemPedido.NF where codLogin = @codLogin";

            // Transforma em um comando SQL com a classe de conexão e adiciona os parâmetros
            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@codLogin", codLogin);

            // Lê os dados retornados pelo comando SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Cria a Lista de Pedidos Vazia
            List<Pedido> pedidos = new List<Pedido>();

            while (dr.Read())
            {
                // Verifica se já existe um pedido com a NF atual na lista
                Pedido pedido = pedidos.FirstOrDefault(p => p.NF == Convert.ToInt32(dr["NF"]));

                // Se o pedido não existir, cria um novo
                if (pedido == null)
                {
                    pedido = new Pedido()
                    {
                        NF = Convert.ToInt32(dr["NF"]),
                        data = Convert.ToDateTime(dr["data"]),
                        statusPedido = Convert.ToInt32(dr["statusPedido"]),
                        valorTotal = Convert.ToDecimal(dr["valorTotal"]),
                        itensPedido = new List<ItemPedido>()
                    };
                    pedidos.Add(pedido);
                }

                // Cria o item do pedido e adiciona à lista de itens do pedido
                ItemPedido item = new ItemPedido()
                {
                    codItem = Convert.ToInt32(dr["codItem"]),
                    produto = new Produto() { codProd = Convert.ToDecimal(dr["codProd"]) }, // Inicialize o produto
                    qtdItem = Convert.ToInt32(dr["qtdItem"]),
                    valorItem = Convert.ToDecimal(dr["valorItem"])
                };
                pedido.itensPedido.Add(item);
            }

            dr.Close();
            conexao.Close();

            return pedidos;
        }

        public List<Pedido> ConsultarPedidosFiltros(int codLogin, int statusPedido)
        {
            // Cria a variável de conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Select tbPedido.*, codItem, codprod, qtdItem, valorItem  from tbPedido Join tbItemPedido On tbPedido.NF = tbItemPedido.NF where codLogin = @codLogin and statusPedido = @statusPedido";

            // Transforma em um comando SQL com a classe de conexão e adiciona os parâmetros
            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@statusPedido", statusPedido);

            // Lê os dados retornados pelo comando SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Cria a Lista de Pedidos Vazia
            List<Pedido> pedidos = new List<Pedido>();

            while (dr.Read())
            {
                // Verifica se já existe um pedido com a NF atual na lista
                Pedido pedido = pedidos.FirstOrDefault(p => p.NF == Convert.ToInt32(dr["NF"]));

                // Se o pedido não existir, cria um novo
                if (pedido == null)
                {
                    pedido = new Pedido()
                    {
                        NF = Convert.ToInt32(dr["NF"]),
                        data = Convert.ToDateTime(dr["data"]),
                        statusPedido = Convert.ToInt32(dr["statusPedido"]),
                        valorTotal = Convert.ToDecimal(dr["valorTotal"]),
                        itensPedido = new List<ItemPedido>()
                    };
                    pedidos.Add(pedido);
                }

                // Cria o item do pedido e adiciona à lista de itens do pedido
                ItemPedido item = new ItemPedido()
                {
                    codItem = Convert.ToInt32(dr["codItem"]),
                    produto = new Produto() { codProd = Convert.ToDecimal(dr["codProd"]) }, // Inicialize o produto
                    qtdItem = Convert.ToInt32(dr["qtdItem"]),
                    valorItem = Convert.ToDecimal(dr["valorItem"])
                };
                pedido.itensPedido.Add(item);
            }

            dr.Close();
            conexao.Close();

            return pedidos;
        }

        public Pedido ConsultarDetalhesPedido(int NF)
        {
            // Cria a variável de conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spSelectDetalhesPedido(@NF);";

            // Junta o comando SQL com a informações do banco   
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribui valor aos parâmetros do comando SQL
            cmd.Parameters.AddWithValue("@NF", NF);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Cria um objeto Pedido null, estou criando aqui pois será usado fora da condicional
            Pedido pedido = null;

            // Se o resultado for legível
            if (dr.Read())
            {
                pedido = new Pedido(
                    Convert.ToInt32(dr["NF"]),
                    Convert.ToDateTime(dr["data"]),
                    Convert.ToInt32(dr["statusPedido"]),
                    Convert.ToDecimal(dr["valorTotal"]),
                    dr["itensPedido"].ToString()
                );
            }

            // Fecha o leitor do produto
            dr.Close();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna o pedido (ou null se não for encontrado)
            return pedido;
        }
    }
}