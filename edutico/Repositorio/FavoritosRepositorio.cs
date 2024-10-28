using edutico.Data;
using edutico.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{

    public class FavoritosRepositorio : IFavoritosRepositorio
    {
        public IEnumerable<Favoritos> ConsultarFavoritos(int codLogin)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Select * from tbFavorito Inner Join vwProduto On tbFavorito.codProd = Código Left Join tbImagem On Código = tbImagem.codProd where codLogin = @codLogin;";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);

            // Lê os dados retornados pelo comando SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Dicionário que guarda os itens do carrinho sem duplicar produtos
            Dictionary<decimal, Favoritos> favoritosDict = new Dictionary<decimal, Favoritos>();

            while (dr.Read())
            {
                decimal codProd = Convert.ToDecimal(dr["Código"]);

                // Verifica se o produto já foi adicionado ao carrinho
                if (!favoritosDict.ContainsKey(codProd))
                {
                    // Se o produto não está no dicionário, cria uma nova instância do carrinho
                    Produto produto = new Produto()
                    {
                        codProd = codProd,
                        nomeProd = dr["Nome"].ToString(),
                        descricao = dr["Descrição"].ToString(),
                        classificacao = dr["Classificação Indicativa"].ToString(),
                        categoria = dr["Categoria"].ToString(),
                        valorUnit = Convert.ToDecimal(dr["Valor Unitário"]),
                        estoque = Convert.ToInt32(dr["Estoque"]),
                        statusProd = Convert.ToBoolean(dr["Status"]),
                        lancamento = Convert.ToBoolean(dr["Lançamento"]),
                        imgs = new List<Imagem>()
                    };

                    Favoritos favoritos = new Favoritos()
                    {
                        codLogin = codLogin,
                        produto = produto
                    };

                    // Adiciona o favoritos ao dicionário
                    favoritosDict.Add(codProd, favoritos);
                }

                // Adiciona a imagem ao produto correspondente, se existir
                if (!dr.IsDBNull(dr.GetOrdinal("nomeImg")))
                {
                    Imagem imagem = new Imagem()
                    {
                        nomeImg = dr["nomeImg"].ToString(),
                        enderecoImg = dr["enderecoImg"].ToString(),
                        codProd = codProd
                    };

                    // Adiciona a imagem ao produto no dicionário
                    favoritosDict[codProd].produto.imgs.Add(imagem);
                }
            }

            dr.Close();
            con.DesconectarBD();

            // Retorna os valores do dicionário como uma lista de carrinhos
            return favoritosDict.Values;
        }
        public void Favoritar(int codLogin, decimal codProd)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spInsertTbFavorito(@codLogin, @codProd);";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codProd", codProd);

            // Executando os comandos do mysql e passsando paa a variavel dr
            cmd.ExecuteNonQuery();


            // deconecta do banco de dados
            con.DesconectarBD();
        }

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


            // Executando os comandos do mysql e passsando paa a variavel dr
            cmd.ExecuteReader();


            // deconecta do banco de dados
            con.DesconectarBD();
        }
    }
}
