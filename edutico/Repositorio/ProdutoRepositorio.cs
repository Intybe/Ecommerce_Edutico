using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {

        public string CadastrarProduto(Produto produto, List<IFormFile> imgs)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spInsertTbProduto(@codprod, @nomeProd, @descricao, @Classificacao, @Categoria, @valorUnit, @estoque);";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@codProd", produto.codProd);
            cmd.Parameters.AddWithValue("@nomeProd", produto.nomeProd);
            cmd.Parameters.AddWithValue("@descricao", produto.descricao);
            cmd.Parameters.AddWithValue("@Classificacao", produto.classificacao);
            cmd.Parameters.AddWithValue("@Categoria", produto.categoria);
            cmd.Parameters.AddWithValue("@valorUnit", produto.valorUnit);
            cmd.Parameters.AddWithValue("@estoque", produto.estoque);

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

            if(mensagem == "Produto Cadastrado com sucesso!")
            {
                foreach (var img in imgs)
                {
                    string nomeImg = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    string caminhoImg = Path.Combine("wwwroot/imgs", nomeImg);

                    using (var stream = new FileStream(caminhoImg, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }

                    string sqlImagem = "Call spInsertTbImagem(@codProd, @nomeImg, @enderecoImg);";

                    cmd = new MySqlCommand(sqlImagem, conexao);
                    cmd.Parameters.AddWithValue("@nomeImg", nomeImg);
                    cmd.Parameters.AddWithValue("@enderecoImg", caminhoImg);
                    cmd.Parameters.AddWithValue("@codProd", produto.codProd);

                    cmd.ExecuteNonQuery();
                }
            }
            return mensagem;


            con.DesconectarBD();

        }
    }
}
