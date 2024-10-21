using edutico.Data;
using edutico.Models;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {

        public string CadastrarProduto(Produto produto, List<IFormFile> imgs, List<string> habilidades)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spInsertTbProduto(@codprod, @nomeProd, @descricao, @Classificacao, @Categoria, @valorUnit, @estoque, @lancamento);";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@codProd", produto.codProd);
            cmd.Parameters.AddWithValue("@nomeProd", produto.nomeProd);
            cmd.Parameters.AddWithValue("@descricao", produto.descricao);
            cmd.Parameters.AddWithValue("@Classificacao", produto.classificacao);
            cmd.Parameters.AddWithValue("@Categoria", produto.categoria);
            cmd.Parameters.AddWithValue("@valorUnit", produto.valorUnit);
            cmd.Parameters.AddWithValue("@estoque", produto.estoque);
            cmd.Parameters.AddWithValue("@lancamento", produto.lancamento);

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

            if (mensagem == "Produto Cadastrado com sucesso!")
            {
                foreach (var img in imgs)
                {
                    string nomeImg = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    string caminhoImg = Path.Combine("wwwroot/imgs", nomeImg);
                    string caminhoImgBD = Path.Combine("~/imgs/", nomeImg);

                    using (var stream = new FileStream(caminhoImg, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }

                    string sqlImagem = "Call spInsertTbImagem(@codProd, @nomeImg, @enderecoImg);";

                    cmd = new MySqlCommand(sqlImagem, conexao);
                    cmd.Parameters.AddWithValue("@nomeImg", nomeImg);
                    cmd.Parameters.AddWithValue("@enderecoImg", caminhoImgBD);
                    cmd.Parameters.AddWithValue("@codProd", produto.codProd);

                    cmd.ExecuteNonQuery();
                }
            }

            foreach (var habilidade in habilidades)
            {
                string sqlHabilidade = "Call spInsertTbHabilidade_Produto(@codProd, @habilidade);";

                cmd = new MySqlCommand(sqlHabilidade, conexao);
                cmd.Parameters.AddWithValue("@codProd", produto.codProd);
                cmd.Parameters.AddWithValue("@habilidade", habilidade.Trim());

                cmd.ExecuteNonQuery();
            }

            return mensagem;


            con.DesconectarBD();

        }

        public IEnumerable<Produto> ConsultarProdutoLancamento()
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Comando SQL para selecionar produtos e suas respectivas imagens
            string sql = "Select * from vwProduto Left Join tbImagem On Código = tbImagem.codProd where Status = 1 and Lançamento = 1;";
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

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
                            imgs = new List<Imagem>() // Inicializa a lista de imagens
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
                        if (drAvaliacoes.Read())
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
                            produto.totalAvaliacao = Convert.ToInt32(drAvaliacoes["total"]);
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

        public Produto ConsultarDetalheProduto(decimal codProd)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Select * from vwProduto Left Join tbImagem On Código = tbImagem.codProd Left Join tbAvaliacao On Código = tbAvaliacao.codProd where Código = @codprod;";

            MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@codProd", codProd);

            // Lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Instancia um objeto Produto para armazenar os dados retornados
            Produto produto = null;

            // Enquanto houver linhas retornadas
            while (dr.Read())
            {
                // Verifica se já existe um produto instanciado
                if (produto == null)
                {
                    // Cria uma nova instância do produto com os dados retornados
                    produto = new Produto()
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
                        imgs = new List<Imagem>(),// Inicializa a lista de imagens
                        avaliacoes = new List<Avaliacao>() // Inicializa a lista de avaliações

                    };
                }

                // Verifica se há uma imagem associada ao produto
                if (!dr.IsDBNull(dr.GetOrdinal("nomeImg")))
                {
                    Imagem imagem = new Imagem()
                    {
                        nomeImg = dr["nomeImg"].ToString(),
                        enderecoImg = dr["enderecoImg"].ToString(),
                        codProd = codProd
                    };

                    // Adiciona a imagem à lista de imagens do produto
                    produto.imgs.Add(imagem);
                }

                // Verifica se há uma avaliação associada ao produto
                if (!dr.IsDBNull(dr.GetOrdinal("qtdEstrela")))
                {
                    Avaliacao avaliacao = new Avaliacao()
                    {
                        codLogin = Convert.ToInt32(dr["codLogin"]),
                        comentario = dr["comentario"].ToString(),
                        qtdEstrela = Convert.ToInt32(dr["qtdEstrela"])
                    };

                    // Adiciona a avaliação à lista de avaliações do produto
                    produto.avaliacoes.Add(avaliacao);
                }
            }

            // Fecha o leitor do produto
            dr.Close();

            // Agora, vamos buscar as métricas de avaliação para este produto
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
            cmdAvaliacoes.Parameters.AddWithValue("@codProd", codProd);

            // Lê os dados retornados pela query de avaliação
            MySqlDataReader drAvaliacoes = cmdAvaliacoes.ExecuteReader();

            // Inicializa o dicionário de avaliações
            produto.dadosAvaliacoes = new Dictionary<int, int>();

            // Verifica se há resultados da consulta de avaliações
            if (drAvaliacoes.Read())
            {
                // Para cada categoria de estrelas de 1 a 5, atribui a quantidade correspondente
                for (int i = 5; i >= 1; i--)
                {
                    // Supondo que 'produto.dadosAvaliacoes' seja o seu dicionário
                    int chave = Convert.ToInt32(i); // A chave do dicionário
                    int valor = Convert.ToInt32(drAvaliacoes[$"estrelas{i}"]); // O valor a ser inserido

                    // Inserindo no dicionário
                    produto.dadosAvaliacoes[chave] = valor;
                }
                produto.somaAvaliacao = Convert.ToInt32(drAvaliacoes["somaAvaliacao"]);
                produto.totalAvaliacao = Convert.ToInt32(drAvaliacoes["total"]);
            }

            // Fecha o leitor de avaliações e a conexão com o banco de dados
            drAvaliacoes.Close();
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

            // Atribuindo valores aos parâmetros
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

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
                            imgs = new List<Imagem>() // Inicializa a lista de imagens
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
                        if (drAvaliacoes.Read())
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
                            produto.totalAvaliacao = Convert.ToInt32(drAvaliacoes["total"]);
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