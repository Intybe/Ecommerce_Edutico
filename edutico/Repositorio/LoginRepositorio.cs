using MySql.Data.MySqlClient;
using edutico.Models;
using System.Data;
using edutico.Data;

namespace edutico.Repositorio
{
    public class LoginRepositorio : ILoginRepositorio
    {

        public Login Login(string usuario, string senha)
        {
            // Criando objeto (variavel) que recebe da classe Conexao
            Conexao conexao = new Conexao();
            {

                // Criando variável para armazenar os comandos SQL
                string sql = "Call spSelectTbLogin(@usuario, @senha)";

                // Variável (objeto) que recebe o comando SQL e executa. Para isso, conecta-se no Banco
                MySqlCommand cmd = new MySqlCommand(sql, conexao.ConectarBD());

                // Passando os parâmetros da consulta 
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@senha", senha);


                // Lê os dados retornados pela procedure do BD
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                // Armazena os dados retornados do Banco de Dados
                MySqlDataReader dr;

                // Executando os comandos do mysql e passsando paa a variavel dr
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Criado um objeto login com valor nulo
                Login login = null;

                // Verifica todos os dados que foram pego do banco
                while (dr.Read())
                {
                    // Instancioando um objeto login
                    login = new Login();

                    // Atruindo os retornos do BD para os atributos do objeto login
                    login.codLogin = Convert.ToInt32(dr["codLogin"]);
                    login.nivelAcesso = Convert.ToInt32(dr["nivelAcesso"]);
                }
                // Fecha a conexão com o Banco de Dados
                conexao.DesconectarBD();
                return login;
            }   
        }

        public decimal ObterCodLoginPorUsuario(string usuario)
        {
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            string sql = "SELECT codLogin FROM tbLogin WHERE usuario = @usuario";
            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@usuario", usuario);

            object result = cmd.ExecuteScalar();
            con.DesconectarBD();

            if (result != null)
            {
                return Convert.ToDecimal(result);
            }
            else
            {
                throw new Exception("Usuário não encontrado.");
            }
        }



        public void AlterarSenha(decimal codLogin, string Senha)
        {
            // Cria variável de conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que recebe o comando SQL
            string sql = "Call spUptade_SenhaCli(@codLogin, @Senha);";

            // Junta o comando SQL com as informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribuindo valores aos parâmetros
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@Senha", Senha);

            // Executa o comando no banco de dados
            cmd.ExecuteNonQuery();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();
        }

    }
}