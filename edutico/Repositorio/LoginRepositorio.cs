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

    }
}