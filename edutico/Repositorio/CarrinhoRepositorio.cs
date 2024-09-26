using edutico.Data;
using edutico.Models;
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
            string sql = "Select * from tbCarrinho Inner Join vwProduto On tbCarrinho.codProd = Código Left Join tbImagem On Código = tbImagem.codProd where codLogin = @codLogin;";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@codLogin", codLogin);

            // Lê os dados retornados pelo comando SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Dicionário que guarda os itens do carrinho sem duplicar produtos
            Dictionary<decimal, Carrinho> carrinhoDict = new Dictionary<decimal, Carrinho>();

            while (dr.Read())
            {
                decimal codProd = Convert.ToDecimal(dr["Código"]);

                // Verifica se o produto já foi adicionado ao carrinho
                if (!carrinhoDict.ContainsKey(codProd))
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

                    Carrinho carrinho = new Carrinho()
                    {
                        codLogin = codLogin,
                        produto = produto,
                        qtdProd = Convert.ToInt32(dr["qtdProd"])
                    };

                    // Adiciona o carrinho ao dicionário
                    carrinhoDict.Add(codProd, carrinho);
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
                    carrinhoDict[codProd].produto.imgs.Add(imagem);
                }
            }

            dr.Close();
            con.DesconectarBD();

            // Retorna os valores do dicionário como uma lista de carrinhos
            return carrinhoDict.Values;
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
    }
}
