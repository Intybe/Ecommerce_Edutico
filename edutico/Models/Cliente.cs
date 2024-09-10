using edutico.Data;
using MySql.Data.MySqlClient;

namespace edutico.Models
{
    public class Cliente
    {
        public int codCli { get; set; }
        public decimal cpf { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string email { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public int numEnd { get; set; }
        public string compEnd { get; set; }


        public void cadastrarProduto(
            int codCli,
            decimal cpf,
            string nome,
            string sobrenome,
            string email,
            string logradouro,
            string cidade,
            string uf,
            string cep,
            int numEnd,
            string compEnd)
        {

            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            string sql = "Call spInsertTbCliente(@codCli, @cpf, @nome, @sobrenome, @email, @logradouro, @cidade, @uf, @cep, @numEnd, @compEnd)";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@codCl", codCli);
            cmd.Parameters.AddWithValue("@cpf", cpf);
            cmd.Parameters.AddWithValue("@nome", nome);
            cmd.Parameters.AddWithValue("@sobrenome", sobrenome);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@logradouro", logradouro);
            cmd.Parameters.AddWithValue("@cidade", cidade);
            cmd.Parameters.AddWithValue("@uf", uf);
            cmd.Parameters.AddWithValue("@cep", cep);
            cmd.Parameters.AddWithValue("@numEnd", numEnd);
            cmd.Parameters.AddWithValue("@compEnd", compEnd);

            cmd.ExecuteNonQuery();

            con.DesconectarBD();

        }

    }
}
