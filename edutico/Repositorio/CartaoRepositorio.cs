using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{
    public class CartaoRepositorio : ICartaoRepositorio
    {
        public String CadastrarCartao(decimal NumCartao, string NomeTitular, int DataVali, string Bandeira, int CodLogin)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spInsertTbCartaoCredito(@NumCartao, @NomeTitular,@DataVali, @Bandeira, @CodLogin);";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@NumCartao", NumCartao);
            cmd.Parameters.AddWithValue("@NomeTitular", NomeTitular);
            cmd.Parameters.AddWithValue("@DataVali", DataVali);
            cmd.Parameters.AddWithValue("@Bandeira", Bandeira);
            cmd.Parameters.AddWithValue("@CodLogin", CodLogin);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();
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

            // deconecta do banco de dados
            con.DesconectarBD();

            return mensagem;
        }




        public List<Cartao> ConsultarCartao(int CodLogin)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spSelectTbCartoes(@codLogin);";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@codLogin", CodLogin);

            // Lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Instancia uma lista de Cartao para armazenar os cartões retornados
            List<Cartao> cartoes = new List<Cartao>();

            // Enquanto houver linhas retornadas
            while (dr.Read())
            {
                // Cria uma nova instância do cartão para cada linha retornada
                Cartao cartao = new Cartao()
                {
                    codCartao = Convert.ToInt32(dr["codCartao"]),
                    codLogin = CodLogin,
                    numCartao = Convert.ToDecimal(dr["numCartao"]),
                    nomeTitular = dr["nomeTitular"].ToString(),
                    dataVali = dr["dataVali"].ToString(),
                    bandeira = dr["bandeira"].ToString(),
                };

                // Adiciona o cartão à lista
                cartoes.Add(cartao);
            }

            // Fecha o leitor e a conexão com o banco de dados
            dr.Close();
            con.DesconectarBD();

            // Retorna a lista de cartões
            return cartoes;
        }
    }
}
