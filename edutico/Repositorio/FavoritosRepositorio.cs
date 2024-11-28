using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{

    public class FavoritosRepositorio : IFavoritosRepositorio
    {
        // Método para consultar os produtos favoritados
        public IEnumerable<Favorito> ConsultarFavoritos(int codLogin)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spSelectFavoritos(@codLogin);";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribuindo valores aos parâmetros
            cmd.Parameters.AddWithValue("@codLogin", codLogin);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Cria uma lista de objeto do tipo favorito
            List<Favorito> favoritos = new List<Favorito>();

            // Enquanto houver linhas cria um objeto produto e adiciona a lista
            while (dr.Read())
            {
                Favorito favorito = new Favorito()
                {
                    codLogin = Convert.ToInt32(dr["codLogin"]),
                    produto = new Produto(
                        Convert.ToDecimal(dr["codProd"]),
                        dr["nomeProd"].ToString(),
                        Convert.ToDecimal(dr["valorUnit"]),
                        Convert.ToInt32(dr["qtdAvaliacao"]),
                        Convert.ToInt32(dr["somaAvaliacao"]),
                        dr["imgs"]?.ToString(),
                        Convert.ToBoolean(dr["statusProd"]),
                        dr["classificacao"].ToString()
                    )
                };

                favoritos.Add(favorito);
            }

            // Fecha o leitor do produto
            dr.Close();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna a lista de favoritos
            return favoritos;
        }


        // Método para favoritar algum produto
        public void Favoritar(int codLogin, decimal codProd)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spInsertTbFavorito(@codLogin, @codProd);";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribuindo valores aos parâmetros
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codProd", codProd);

            // Executando o comando SQL sem retorno de resultado
            cmd.ExecuteNonQuery();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();
        }


        // Método para Desfavoritar algum produto
        public void RemoverFavorito(int codLogin, decimal codProd)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spDeleteTbFavorito(@codLogin, @codProd);";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codProd", codProd);

            // Executando o comando SQL sem retorno de resultado
            cmd.ExecuteNonQuery();

            // deconecta do banco de dados
            con.DesconectarBD();
        }
    }
}
