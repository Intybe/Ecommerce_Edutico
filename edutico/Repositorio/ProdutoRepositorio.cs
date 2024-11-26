using edutico.Data;
using edutico.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

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

            // obrigatório 
            string mensagem = null;

            //dr = recebe e verifca se é "legivel os dados 
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
                    dr["imgs"].ToString(),
                     statusProd: Convert.ToBoolean(dr["statusProd"])
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



        //Consultar Produto Funcionário
        public IEnumerable<Produto> ConsultarProdutoF()
        {
            //var para conectar o bd 
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // comando sql para selecionar os produtos e suas imgs
            string sql = "Call spSelectPreviaProdutoF();";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            //  executa e le oq veio da query 
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
                    dr["imgs"].ToString(),
                     statusProd: Convert.ToBoolean(dr["statusProd"])
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
                    imgsConcatenadas: dr["imgs"].ToString(),
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
                    dr["imgs"]?.ToString(),
                    statusProd: Convert.ToBoolean(dr["statusProd"])
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


        //DETAKHES PRODUTO FUNCIONARIO


        // Método de consulta aos detalhes do produto
        public Produto ConsultarDetalheProdutoF(decimal codProd)
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
                    imgsConcatenadas: dr["imgs"].ToString(),
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
                    dr["imgs"]?.ToString(),
                    statusProd: Convert.ToBoolean(dr["statusProd"])
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


        public Avaliacao ConsultarAvaliacaoCliente(decimal codProd, int codLogin)
        {
            // Cria a variável de conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spSelectTbAvaliacaoUnica(@codProd, @codLogin);";

            // Junta o comando SQL com a informações do banco   
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribui valor aos parâmetros do comando SQL
            cmd.Parameters.AddWithValue("@codProd", codProd);
            cmd.Parameters.AddWithValue("@codLogin", codLogin);

            // Executa e le os dados retornados da query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            //criando um obj do tipo avaliacao 
            Avaliacao avaliacaoUnica = null;

            if (dr.Read()) //se o valor forem legiveis 
            {
                avaliacaoUnica = new Avaliacao() //dando valor pro obj 
                {
                    codProd = Convert.ToDecimal(dr["codProd"]),
                    codAvaliacao = Convert.ToInt32(dr["codAvaliacao"]),
                    comentario = dr["comentario"].ToString(),
                    qtdEstrela = Convert.ToInt32(dr["qtdEstrela"]),
                    cliente = new Cliente { codLogin = Convert.ToInt32(dr["codLogin"]) }
                };

            }

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna o produto (ou null se não for encontrado)
            return avaliacaoUnica;
        }


        public string DeletarAvaliacao(int codLogin, int codAvaliacao, decimal codProd)
        {
            // Cria a variável de conexão com o banco de dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spDeleteTbAvaliacao(@codLogin, @codAvaliacao);";

            // Junta o comando SQL com a informações do banco   
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribui valor aos parâmetros do comando  SQL
            cmd.Parameters.AddWithValue("@codLogin", codLogin);
            cmd.Parameters.AddWithValue("@codAvaliacao", codAvaliacao);

            string mensagem = null;
            // Executa a query SQL
            cmd.ExecuteNonQuery();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            return mensagem;
        }

        public IEnumerable<Produto> ConsultarProdutoPesquisa(string pesquisa)
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Comando SQL para selecionar produtos e suas respectivas imagens, com filtro de pesquisa
            string sql = "Select * from vwPreviaProduto where statusProd = 1 and nomeProd like @pesquisa;"; // Filtro de pesquisa            

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribui valor aos parâmetros do comando SQL
            cmd.Parameters.AddWithValue("@pesquisa", "%" + pesquisa + "%");

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

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
                    dr["imgs"]?.ToString(),
                    statusProd: Convert.ToBoolean(dr["statusProd"])
                );
                produtos.Add(produtoRelacionados);
            }

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna o produto (ou null se não for encontrado)
            return produtos;
        }


        public void AtualizarStatusProduto(decimal codProd, int statusProd)
        {
            // Cria variável de conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que recebe o comando SQL
            string sql = "Call spUpdateStatusTbProduto(@codProd, @statusProd);";

            // Junta o comando SQL com as informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Atribuindo valores aos parâmetros
            cmd.Parameters.AddWithValue("@codProd", codProd);
            cmd.Parameters.AddWithValue("@statusProd", statusProd);

            // Executa o comando no banco de dados
            cmd.ExecuteNonQuery();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();
        }

        public Dashboard ConsultarInfosEcommerce()
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Select * from vwInfosEcommerce;";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Criando um objeto do tipo Dashboard
            Dashboard infos = new Dashboard();

            if (dr.Read())
            {
                infos.pedidosPendentes = Convert.ToInt32(dr["pendentes"]);
                infos.qtdFavoritos = Convert.ToInt32(dr["qtdFavoritos"]);
                infos.qtdVendasAtual = Convert.ToInt32(dr["VendasMesAtual"]);
                infos.qtdVendasAnterior = Convert.ToInt32(dr["VendasMesAnterior"]);
                infos.produtosEsgotados = Convert.ToInt32(dr["ProdutosEsgotados"]);
            }

            // Fechar o DataReader antes de executar outro comando
            dr.Close();

            // Vairável que recebe o comando SQL
            sql = "Call spSelectDashboardProdutos();";

            // Junta o comando SQL com a informações do banco
            cmd = new MySqlCommand(sql, conexao);

            // Executa e lê os dados retornados pela query SQL
            dr = cmd.ExecuteReader();

            // Cria uma lista de objetos Produtos
            infos.maisVendidos = new List<Produto>();

            // Enquanto houver resultados
            while (dr.Read())
            {
                // Cria um objeto produto usando construtor
                Produto produto = new Produto(
                    Convert.ToDecimal(dr["codProd"]),
                    dr["nomeProd"].ToString(),
                    Convert.ToDecimal(dr["valorUnit"]),
                    dr["img"].ToString(),
                    Convert.ToInt32(dr["qtdVendas"]),
                    Convert.ToDecimal(dr["valorVendas"])
                );

                // Adiciona o produto na Lista 
                infos.maisVendidos.Add(produto);
            }

            // Fechar o DataReader
            dr.Close();

            // Vairável que recebe o comando SQL
            sql = "Select * from vwdadosGrafico;";

            // Junta o comando SQL com a informações do banco
            cmd = new MySqlCommand(sql, conexao);

            // Executa e lê os dados retornados pela query SQL
            dr = cmd.ExecuteReader();

            // Inicializa as listas
            infos.vendaDiaria = new List<decimal>();
            infos.vendaSemanal = new List<decimal>();
            infos.vendaMensal = new List<decimal>();

            // Enquanto houver resultados
            while (dr.Read())
            {
                // Adiciona os valores na lista decimal, sendo dom o índice zero e assim por diante
                infos.vendaDiaria.Add(Convert.ToDecimal(dr["dom"]));
                infos.vendaDiaria.Add(Convert.ToDecimal(dr["seg"]));
                infos.vendaDiaria.Add(Convert.ToDecimal(dr["ter"]));
                infos.vendaDiaria.Add(Convert.ToDecimal(dr["qua"]));
                infos.vendaDiaria.Add(Convert.ToDecimal(dr["qui"]));
                infos.vendaDiaria.Add(Convert.ToDecimal(dr["sex"]));
                infos.vendaDiaria.Add(Convert.ToDecimal(dr["sab"]));
                infos.totalVendaSemanal = infos.vendaDiaria.Sum();

                // Adiciona os valores na lista decimal, sendo semana1 o índice zero e assim por diante
                infos.vendaSemanal.Add(Convert.ToDecimal(dr["semana1"]));
                infos.vendaSemanal.Add(Convert.ToDecimal(dr["semana2"]));
                infos.vendaSemanal.Add(Convert.ToDecimal(dr["semana3"]));
                infos.vendaSemanal.Add(Convert.ToDecimal(dr["semana4"]));
                infos.vendaSemanal.Add(Convert.ToDecimal(dr["semana5"]));
                infos.totalVendaMensal = infos.vendaSemanal.Sum();

                // Adiciona os valores na lista decimal, sendo janeiro o índice zero e assim por diante
                infos.vendaMensal.Add(Convert.ToDecimal(dr["janeiro"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["fevereiro"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["marco"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["abril"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["maio"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["junho"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["julho"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["agosto"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["setembro"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["outubro"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["novembro"]));
                infos.vendaMensal.Add(Convert.ToDecimal(dr["dezembro"]));
                infos.totalVendaAnual = infos.vendaMensal.Sum();
            }

            // Fechar o DataReader
            dr.Close();

            // Fecha a conexão com o banco de dados
            con.DesconectarBD();

            // Retorna as informações necessários do Dashboard
            return infos;
        }

        public List<Produto> ConsultarProdutosFavoritos()
        {
            // Cria variável de Conexão com o Banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Vairável que recebe o comando SQL
            string sql = "Call spSelectFavoritosFuncionario();";

            // Junta o comando SQL com a informações do banco
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Executa e lê os dados retornados pela query SQL
            MySqlDataReader dr = cmd.ExecuteReader();

            // Criando um objeto do tipo Dashboard
            List<Produto> produtos = new List<Produto>();

            while (dr.Read())
            {
                Produto produto = new Produto(
                    Convert.ToDecimal(dr["codProd"]),
                    dr["nomeProd"].ToString(),
                    Convert.ToDecimal(dr["valorUnit"]),
                    dr["img"]?.ToString(),
                    Convert.ToInt32(dr["qtdfavorito"])
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
    }
}
