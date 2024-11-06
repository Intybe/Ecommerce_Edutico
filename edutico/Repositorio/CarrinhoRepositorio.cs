using edutico.Data;
using edutico.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{

    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        // Método para consultar os itens do carrinho
        public IEnumerable<Carrinho> ConsultarCarrinho(int codLogin)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spSelectCarrinho(@codLogin)";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);

            // Lê os dados retornados pelo comando SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Dicionário que guarda os itens do carrinho sem duplicar produtos
            List<Carrinho> itensCarrinho = new List<Carrinho>();

            while (dr.Read())
            {
                // Se o produto não está no dicionário, cria uma nova instância do carrinho
                Carrinho carrinho = new Carrinho(
                    codLogin,
                    Convert.ToDecimal(dr["codProd"]),
                    dr["nomeProd"].ToString(),
                    Convert.ToDecimal(dr["valorUnit"]),
                    dr["imgs"]?.ToString(),
                    Convert.ToInt32(dr["qtdProd"])
                );

                // Adiciona o carrinho ao dicionário
                itensCarrinho.Add(carrinho);
            }

            dr.Close();
            con.DesconectarBD();

            // Retorna os valores do dicionário como uma lista de carrinhos
            return itensCarrinho;
        }

        // Método paea Cadastrar os produtos ao carrinho
        public String CadastrarProdutoCarrinho(int codLogin, decimal codProd, int qtdProd)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spInsertTbCarrinho(@codLogin, @codProd, @qtdProd);";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codProd", codProd);
            cmd.Parameters.AddWithValue("@qtdProd", qtdProd);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            // Para não dar erro na mensagem 
            string mensagem = null;

            if (dr.Read())
            {
                mensagem = dr.GetString(0); // Captura a primeira coluna (que é a mensagem retornada)
            }
            else
            {
                mensagem = "Erro Desconhecido";
            }
            // Fechar o DataReader antes de executar outro comando
            dr.Close();

            return mensagem;

            // deconecta do banco de dados
            con.DesconectarBD();
        }

        public string ExcluirItemCarrinho(int codLogin, decimal codProd)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spDeleteTbCarrinho(@codLogin, @codProd);";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codProd", codProd);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            // Para não dar erro na mensagem 
            string mensagem = null;

            if (dr.Read())
            {
                mensagem = dr.GetString(0); // Captura a primeira coluna (que é a mensagem retornada)
            }
            else
            {
                mensagem = "Erro Desconhecido";
            }
            // Fechar o DataReader antes de executar outro comando
            dr.Close();

            return mensagem;

            // deconecta do banco de dados
            con.DesconectarBD();
        }
        public string AtualizarQtdProdCarrinho(int codLogin, decimal codProd, int qtdProd)
        {
            // Conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

                // Comando SQL para atualizar a quantidade de produtos no carrinho
                string sql = "call spUpdateTbCarrinho(@codLogin,@codProd, @qtdProd);";

                // Preparação do comando com parâmetros
                MySqlCommand cmd = new MySqlCommand(sql, conexao);
                cmd.Parameters.AddWithValue("@codLogin", codLogin);
                cmd.Parameters.AddWithValue("@codProd", codProd);
                cmd.Parameters.AddWithValue("@qtdProd", qtdProd);

                // Lê os dados retornados pela procedure do BD
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                // Armazena os dados retornados do Banco de Dados
                MySqlDataReader dr;

                // Executando os comandos do mysql e passsando paa a variavel dr
                dr = cmd.ExecuteReader();

                // Para não dar erro na mensagem 
                string mensagem = null;

                if (dr.Read())
                {
                    mensagem = dr.GetString(0); // Captura a primeira coluna (que é a mensagem retornada)
                }
                else
                {
                    mensagem = "Erro Desconhecido";
                }
                // Fechar o DataReader antes de executar outro comando
                dr.Close();

                return mensagem;

                // deconecta do banco de dados
                con.DesconectarBD();
            
        }
    }
}
