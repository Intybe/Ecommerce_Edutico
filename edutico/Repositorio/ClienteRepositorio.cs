using edutico.Data;
using MySql.Data.MySqlClient;
using edutico.Models;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;

namespace edutico.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        public string CadastrarCliente(decimal cpf, string nome, string sobrenome, string telefone, string email, string logradouro, string bairro, string cidade, string uf, string cep, int numEnd, string compEnd, string senha)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spInsertTbCliente(@cpf, @nome, @sobrenome, @telefone, @email, @senha, @logradouro, @bairro, @cidade, @uf, @cep, @numEnd, @compEnd)";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@cpf", cpf);
            cmd.Parameters.AddWithValue("@nome", nome);
            cmd.Parameters.AddWithValue("@sobrenome", sobrenome);
            cmd.Parameters.AddWithValue("@telefone", telefone);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@logradouro", logradouro);
            cmd.Parameters.AddWithValue("@bairro", bairro);
            cmd.Parameters.AddWithValue("@cidade", cidade);
            cmd.Parameters.AddWithValue("@uf", uf);
            cmd.Parameters.AddWithValue("@cep", cep);
            cmd.Parameters.AddWithValue("@numEnd", numEnd);
            cmd.Parameters.AddWithValue("@compEnd", compEnd);
            cmd.Parameters.AddWithValue("@senha", senha);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                string mensagem = dr.GetString(0); // Captura a primeira coluna (que é a mensagem retornada)
                return mensagem; // Retorna a mensagem para quem chamou o método
            }
            else
            {
                string mensagem = "Erro Desconhecido";
                return mensagem;
            }

            con.DesconectarBD();
        }

        public Cliente ConsultarCliente(int codLogin)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Select * from vwCliente where codLogin = @codLogin;";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Adicionando Parâmetros
            cmd.Parameters.AddWithValue("@codLogin", codLogin);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            if (dr.Read()) // Verifica se há registros a serem lidos
            {
                Cliente cliente = new Cliente()
                {
                    codLogin = Convert.ToInt32(dr["codLogin"]),
                    codCli = Convert.ToInt32(dr["Código"]),
                    cpf = Convert.ToDecimal(dr["CPF"]),
                    nome = dr["Nome"].ToString(),
                    sobrenome = dr["Sobrenome"].ToString(),
                    telefone = dr["Telefone"].ToString(),
                    email = dr["Email"].ToString(),
                    logradouro = dr["Logradouro"].ToString(),
                    bairro = dr["Bairro"].ToString(),
                    cidade = dr["Cidade"].ToString(),
                    uf = dr["Estado"].ToString(),
                    cep = dr["CEP"].ToString(),
                    numEnd = Convert.ToInt32(dr["Número"]),
                    compEnd = dr["Complemento"].ToString()
                };

                return cliente;
            }
            else
            {
                // Trate o caso em que nenhum registro foi encontrado
                return null;
            }
        }

        public string AtualizarClienteConta(int codLogin, string email, string senha)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spUpdateTbClienteConta(@codLogin, @email, @senha)";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Adicionando Parâmetros
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@senha", senha);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            // Criando  variável para receber mensagem de retorno
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

            // Retorna para controller mensagem que veio do Banco de Dados
            return mensagem;
        }

        public string AtualizarClienteEndereco(int codLogin, string logradouro, string bairro, string cep, string cidade, string uf, int numEnd, string compEnd)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spUpdateTbClienteEndereco(@codLogin, @logradouro, @bairro, @cidade, @uf, @cep, @numEnd, @compEnd)";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Adicionando Parâmetros
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@logradouro", logradouro);
            cmd.Parameters.AddWithValue("@bairro", bairro);
            cmd.Parameters.AddWithValue("@cidade", cidade);
            cmd.Parameters.AddWithValue("@uf", uf);
            cmd.Parameters.AddWithValue("@cep", cep);
            cmd.Parameters.AddWithValue("@numEnd", numEnd);
            cmd.Parameters.AddWithValue("@compEnd", compEnd);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            // Criando  variável para receber mensagem de retorno
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

            // Retorna para controller mensagem que veio do Banco de Dados
            return mensagem;
        }

        public string AtualizarClienteDados(int codLogin, decimal cpf, string nome, string sobrenome, string telefone)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spUpdateTbClienteDados(@codLogin, @cpf, @nome, @sobrenome, @telefone)";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Adicionando Parâmetros
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@cpf", cpf);
            cmd.Parameters.AddWithValue("@nome", nome);
            cmd.Parameters.AddWithValue("@sobrenome", sobrenome);
            cmd.Parameters.AddWithValue("@telefone", telefone);


            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            // Criando  variável para receber mensagem de retorno
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

            // Retorna para controller mensagem que veio do Banco de Dados
            return mensagem;
        }
    }
}
