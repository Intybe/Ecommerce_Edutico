using edutico.Data;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{
    public class PagamentoRepositorio
    {

        public string PagamentoPix(int nf, int qtdParcela, string codPix)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spInsertTbPagamento(@qtdParcela, @NF, @codPix, @codCartao);";

            // Atribuindo valores aos parâmetros
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@qtdParcela", qtdParcela);
            cmd.Parameters.AddWithValue("@NF", nf);
            cmd.Parameters.AddWithValue("@codPix", codPix);
            cmd.Parameters.AddWithValue("@codCartao", 0);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

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

            con.DesconectarBD();

            return mensagem;
        }

        public string PagamentoCartao(int nf, int qtdParcela, int codCartao)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spInsertTbPagamento(@qtdParcela, @NF, @codPix, @codCartao);";

            // Atribuindo valores aos parâmetros
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@qtdParcela", qtdParcela);
            cmd.Parameters.AddWithValue("@NF", nf);
            cmd.Parameters.AddWithValue("@codPix", 1);
            cmd.Parameters.AddWithValue("@codCartao", codCartao);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

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

            con.DesconectarBD();

            return mensagem;
        }
    }
}
