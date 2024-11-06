using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        // Método para Cadastro de Produto
        public string CadastrarProduto(Produto produto, List<IFormFile> imgs)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que recebe o comando SQL para inserir o produto
            string sql = "Call spInsertTbProduto(@codprod, @nomeProd, @descricao, @codClassificacao, @codCategoria, @valorUnit, @estoque, @lancamento);";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribui valor aos parâmetros do comando SQL
            cmd.Parameters.AddWithValue("@codProd", produto.codProd);
            cmd.Parameters.AddWithValue("@nomeProd", produto.nomeProd);
            cmd.Parameters.AddWithValue("@descricao", produto.descricao);
            cmd.Parameters.AddWithValue("@codClassificacao", produto.classificacao.codClassificacao);
            cmd.Parameters.AddWithValue("@codCategoria", produto.categoria.codCategoria);
            cmd.Parameters.AddWithValue("@valorUnit", produto.valorUnit);
            cmd.Parameters.AddWithValue("@estoque", produto.estoque);
            cmd.Parameters.AddWithValue("@lancamento", produto.lancamento);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            string mensagem = null;

            if (dr.Read())
            {
                if (dr.GetString(0) == "Produto Cadastrado com sucesso!")
                {
                    // Fecha o leitor utilizado para ler a mensagem
                    dr.Close();

                    // Cria a variável para a sequência
                    int i = 1;

                    // Irá executar o código para cada imagem na lista
                    foreach (var img in imgs)
                    {
                        // Deixa o nome do produto sem espaço
                        string nomeProd = produto.nomeProd.Replace(" ", "_");

                        // Atribui o nome do produto, um número e a extenção da imagem
                        string nomeImg = string.Concat(nomeProd, "_", i, Path.GetExtension(img.FileName));

                        // Variável com o caminho para salvar a imagem na pasta
                        string caminhoImg = Path.Combine("wwwroot/imgs/produtos", nomeImg);

                        // Variável para salvar o caminho da imagemm no Banco de Dados
                        string caminhoImgBD = Path.Combine("~/imgs/produtos", nomeImg);

                        // Cria uma instância da classe que permite criar um arquivo
                        var criarImg = new FileStream(caminhoImg, FileMode.Create);

                        // Cria fisicamento a imagem na pasta
                        img.CopyTo(criarImg);

                        // Comando SQL para inserir a imagem associada ao produto
                        string sqlImagem = "Call spInsertTbImagem(@codProd, @nomeImg, @enderecoImg);";

                        // Junta o comando SQL com a informações do banco
                        cmd = new MySqlCommand(sqlImagem, conexao);

                        // Atribui valor aos parâmetros do comando SQL
                        cmd.Parameters.AddWithValue("@codProd", produto.codProd);
                        cmd.Parameters.AddWithValue("@nomeImg", nomeImg);
                        cmd.Parameters.AddWithValue("@enderecoImg", caminhoImgBD);

                        // Executa a query sem retorno
                        cmd.ExecuteNonQuery();

                        // Auemntando valor da variável
                        i++;
                    }

                    foreach (var habilidade in produto.habilidades)
                    {
                        // Comando SQL para inserir as habilidades associadas ao produto
                        string sqlHabilidade = "Call spInsertTbHabilidade_Produto(@codProd, @codHabilidade);";
                        cmd = new MySqlCommand(sqlHabilidade, conexao);
                        cmd.Parameters.AddWithValue("@codProd", produto.codProd);
                        cmd.Parameters.AddWithValue("@codHabilidade", habilidade.codHabilidade);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                mensagem = "Ocorreu um erro inesperado! Tente novamente!";
            }

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna a mensagem
            return mensagem;
        }


        // Método de consultar Produtos em Lançamento
        public IEnumerable<Produto> ConsultarProdutoLancamento()
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Comando SQL para selecionar produtos e suas respectivas imagens
            string sql = "Call spSelectPreviaProduto()";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Cria uma lista de objeto do tipo Produto
            List<Produto> produtos = new List<Produto>();

            // Enquanto houver linhas cria um objeto produto e adiciona a lista
            while (dr.Read())
            {
                Produto produto = new Produto(
                    Convert.ToDecimal(dr["codProd"]),
                    dr["nomeProd"].ToString(),
                    Convert.ToDecimal(dr["valorUnit"]),
                    Convert.ToInt32(dr["qtdAvaliacao"]),
                    Convert.ToInt32(dr["somaAvaliacao"]),
                    dr["imgs"].ToString()
                );
                produtos.Add(produto);
            }

            // Fecha o leitor do produto
            dr.Close();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna o produto (ou null se não for encontrado)
            return produtos;
        }


        // Método de consulta aos detalhes do produto
        public Produto ConsultarDetalheProduto(decimal codProd)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spSelectDetalheProduto(@codProd)";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribui valor aos parâmetros do comando SQL
            cmd.Parameters.AddWithValue("@codProd", codProd);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Cria um objeto Produto null, estou criando aqui pois será usado fora da condicional
            Produto produto = null;

            // Enquanto houver linhas retornadas
            if (dr.Read())
            {
                // Cria um objeto do produto com os dados retornados
                produto = new Produto(
                    codProd: codProd,
                    nomeProd: dr["nomeProd"].ToString(),
                    descricao: dr["descricao"].ToString(),
                    classificacao: dr["classificacao"].ToString(),
                    categoria: dr["categoria"].ToString(),
                    habilidadesConcatenadas: dr["habilidades"]?.ToString(),
                    valorUnit: Convert.ToDecimal(dr["valorUnit"]),
                    estoque: Convert.ToInt32(dr["estoque"]),
                    statusProd: Convert.ToBoolean(dr["statusProd"]),
                    lancamento: Convert.ToBoolean(dr["lancamento"]),
                    imgsConcatenadas: dr["imgs"]?.ToString(),
                    detalhesAvaliacaoConcatenados: dr["detalhesAvaliacao"]?.ToString(),
                    qtdPorEstrelaConcatenada: dr["qtdPorEstrela"]?.ToString(),
                    qtdAvaliacao: Convert.ToInt32(dr["qtdAvaliacao"]),
                    somaAvaliacao: Convert.ToInt32(dr["somaAvaliacao"])
                );
            }

            // Fecha o leitor do produto
            dr.Close();

            // Vairável que recebe o comando SQL
            sql = "Call spSelectRelacionados(@nomeCategoria)";

            // Junta o comando SQL com a informações do banco
            cmd = new MySqlCommand(sql, conexao);

            // Atribui valor aos parâmetros do comando SQL
            cmd.Parameters.AddWithValue("@nomeCategoria", produto.categoria.nomeCategoria);

            // Executa e lê os dados retornados pela query SQL
            dr = cmd.ExecuteReader();

            // Cria uma lista de objeto do tipo Produto
            List<Produto> produtos = new List<Produto>();

            // Enquanto houver linhas cria um objeto produto e adiciona a lista de produtos relacioados
            while (dr.Read())
            {
                Produto produtoRelacionados = new Produto(
                    Convert.ToDecimal(dr["codProd"]),
                    dr["nomeProd"].ToString(),
                    Convert.ToDecimal(dr["valorUnit"]),
                    Convert.ToInt32(dr["qtdAvaliacao"]),
                    Convert.ToInt32(dr["somaAvaliacao"]),
                    dr["imgs"]?.ToString()
                );
                produtos.Add(produtoRelacionados);
            }

            // Adiciona os produtos relacionados
            produto.relacionados = produtos;

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna o produto (ou null se não for encontrado)
            return produto;
        }



        public string CadastrarAvaliacao(int qtdEstrela, string comentario, int codLogin, decimal codProd)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spInsertTbAvaliacao(@qtdEstrela, @comentario, @codlogin, @codProd);";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribuindo valores aos parâmetros
            cmd.Parameters.AddWithValue("@qtdEstrela", qtdEstrela);
            cmd.Parameters.AddWithValue("@comentario", comentario);
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codProd", codProd);

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

        public IEnumerable<Produto> ConsultarProdutoPesquisa(string pesquisa)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Comando SQL para selecionar produtos e suas respectivas imagens, com filtro de pesquisa
            string sql = @"
                Select * from vwProduto 
                Left Join tbImagem On Código = tbImagem.codProd 
                where Status = 1 and Lançamento = 1 
                and (nomeProd like @pesquisa OR descricao like @pesquisa);"; // Filtro de pesquisa

            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@pesquisa", "%" + pesquisa + "%"); // Adiciona o parâmetro de pesquisa com wildcards

            // Dicionário para armazenar os produtos enquanto lemos as imagens
            Dictionary<decimal, Produto> produtoDict = new Dictionary<decimal, Produto>();

            MySqlDataReader dr = null;

            try
            {
                // Lê os dados retornados pelo comando SQL
                dr = cmd.ExecuteReader();

                // Irá repetir o processo para cada linha retornada
                while (dr.Read())
                {
                    decimal codProd = Convert.ToDecimal(dr["Código"]);

                    // Verifica se o produto já foi adicionado ao dicionário
                    if (!produtoDict.ContainsKey(codProd))
                    {
                        // Se o produto ainda não foi adicionado, cria uma nova instância
                        Produto produto = new Produto()
                        {/*
                            codProd = codProd,
                            nomeProd = dr["Nome"].ToString(),
                            descricao = dr["Descrição"].ToString(),
                            classificacao = dr["Classificação Indicativa"].ToString(),
                            categoria = dr["Categoria"].ToString(),
                            valorUnit = Convert.ToDecimal(dr["Valor Unitário"]),
                            estoque = Convert.ToInt32(dr["Estoque"]),
                            statusProd = Convert.ToBoolean(dr["Status"]),
                            lancamento = Convert.ToBoolean(dr["Lançamento"]),
                            imgs = new List<Imagem>() // Inicializa a lista de imagens
                            */
                        };

                        // Adiciona o produto ao dicionário
                        produtoDict.Add(codProd, produto);
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

                        produtoDict[codProd].imgs.Add(imagem);
                    }
                }

                // Fecha o primeiro DataReader antes de abrir outro
                dr.Close();

                // Agora, vamos buscar as métricas de avaliação para cada produto
                foreach (var produto in produtoDict.Values)
                {
                    string sqlAvaliacoes = @"
                        Select 
                            Count(*) AS total, 
                            Sum(qtdEstrela) AS somaAvaliacao,
                            Sum(Case when qtdEstrela = 5 then 1 else 0 END) AS estrelas5,
                            Sum(Case when qtdEstrela = 4 then 1 else 0 END) AS estrelas4,
                            Sum(Case when qtdEstrela = 3 then 1 else 0 END) AS estrelas3,
                            Sum(Case when qtdEstrela = 2 then 1 else 0 END) AS estrelas2,
                            Sum(Case when qtdEstrela = 1 then 1 else 0 END) AS estrelas1
                        from tbAvaliacao 
                        where codProd = @codProd;"; // Filtrando pela codProd

                    MySqlCommand cmdAvaliacoes = new MySqlCommand(sqlAvaliacoes, conexao);
                    cmdAvaliacoes.Parameters.AddWithValue("@codProd", produto.codProd);

                    // Agora podemos abrir um novo DataReader sem conflitos
                    using (MySqlDataReader drAvaliacoes = cmdAvaliacoes.ExecuteReader())
                    {
                        // Inicializa o dicionário de avaliações
                        produto.dadosAvaliacoes = new Dictionary<int, int>();

                        // Verifica se há resultados da consulta de avaliações
                        if (drAvaliacoes.Read() && drAvaliacoes != null &&
    drAvaliacoes["estrelas1"] != DBNull.Value &&
    drAvaliacoes["estrelas2"] != DBNull.Value &&
    drAvaliacoes["estrelas3"] != DBNull.Value &&
    drAvaliacoes["estrelas4"] != DBNull.Value &&
    drAvaliacoes["estrelas5"] != DBNull.Value &&
    drAvaliacoes["somaAvaliacao"] != DBNull.Value &&
    drAvaliacoes["total"] != DBNull.Value)
                        {
                            // Para cada categoria de estrelas de 1 a 5, atribui a quantidade correspondente
                            for (int i = 5; i >= 1; i--)
                            {
                                int chave = i; // A chave do dicionário
                                int valor = Convert.ToInt32(drAvaliacoes[$"estrelas{i}"]); // O valor a ser inserido

                                // Inserindo no dicionário
                                produto.dadosAvaliacoes[chave] = valor;
                            }

                            produto.somaAvaliacao = Convert.ToInt32(drAvaliacoes["somaAvaliacao"]);
                            produto.qtdAvaliacao = Convert.ToInt32(drAvaliacoes["total"]);
                        }
                    }
                }
            }
            finally
            {
                // Fecha a conexão com o banco de dados
                if (dr != null && !dr.IsClosed) dr.Close();
                con.DesconectarBD();
            }

            // Retorna a lista de produtos com suas imagens e avaliações
            return produtoDict.Values;
        }
    }
}