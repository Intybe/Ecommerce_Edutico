using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace edutico.Controllers
{
    public class ProdutoController : Controller
    {
        // instanciando métodos de outras classes para utilizar aq 
        private IProdutoRepositorio? _produtoRepositorio;
        private readonly LoginSessao _loginSessao;
        private IFavoritosRepositorio? _favoritosRepositorio;
        // Construtor com injeção de dependência
        public ProdutoController(IProdutoRepositorio produtoRepositorio, LoginSessao loginSessao, IFavoritosRepositorio favoritosRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _loginSessao = loginSessao;
            _favoritosRepositorio = favoritosRepositorio;
        }

        public IActionResult CadastroProduto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProduto(string nomeProd, string codProd, string descricaoProd, string classificacao, string habilidadesEnviadas, string valorUnit, string estoque, string categoria, string lacamentoProd, List<IFormFile> imgs)
        {
            // Verificar se a lista de imagens está vazia
            if (imgs == null || !imgs.Any())
            {
                ViewData["msg"] = "Por favor, selecione ao menos uma imagem.";
                return View("CadastroProduto");
            }

            // Criar o objeto produto com os dados recebidos
            Produto produto = new Produto(
                Convert.ToDecimal(codProd),
                nomeProd,
                descricaoProd,
                classificacao,
                categoria,
                habilidadesEnviadas,
                Convert.ToDecimal(valorUnit),
                Convert.ToInt32(estoque),
                Convert.ToBoolean(lacamentoProd)
            );

            // Chamar o repositório para salvar o produto e as imagens
            string resultado = _produtoRepositorio.CadastrarProduto(produto, imgs);

            ViewData["msg"] = resultado;
            return View("CadastroProduto");
        }

        public IActionResult DetalhesProduto(decimal? codProd)
        {
            List<decimal> favoritos = new List<decimal>();
            

            Produto produto = null;
            Avaliacao avaliacaoUnica = null;

            if (_loginSessao != null && _loginSessao.GetLogin() != null)
            {
                var login = _loginSessao.GetLogin();

                // Verifica se o codProd é nulo ou não foi passado
                if (!codProd.HasValue)
                {
                    /*
                    // Criar o produto padrão
                    produto = new Produto()
                    {
                        codProd = 1, // Código do produto
                        nomeProd = "Nome Produto Padrão",
                        descricao = "Descrição Padrão",
                        classificacao = "Classificação Indicativa",
                        categoria = "Categoria",
                        valorUnit = 0, // Valor padrão
                        estoque = 100, // Estoque padrão
                        statusProd = true, // Produto ativo
                        lancamento = false, // Não é lançamento
                        imgs = new List<Imagem>() // Inicializa a lista de imagens
                    };

                    // Adiciona múltiplas imagens padrão ao produto
                    for (int i = 1; i <= 5; i++)
                    {
                        Imagem imagem = new Imagem
                        {
                            enderecoImg = "~/imgs/img_prod_padrao_quadrada.png" // Caminhos de imagem diferentes
                        };

                        produto.imgs.Add(imagem); // Adiciona a imagem à lista de imagens do produto
                    }
                    */
                }
                else
                {
                    // Consultar o produto pelo código
                    produto = _produtoRepositorio.ConsultarDetalheProduto(codProd.Value);

                    // Obtém os favoritos do cliente logado
                    var favoritosList = _favoritosRepositorio.ConsultarFavoritos(login.codLogin).ToList();
                   

                    // Extrai os códigos dos produtos favoritos
                    favoritos = favoritosList.Select(f => f.produto.codProd).ToList();
 
                   avaliacaoUnica = _produtoRepositorio.ConsultarAvaliacaoCliente(codProd.Value, login.codLogin);
                    
                }
            }
            else
            {
                // Consultar o produto pelo código
                produto = _produtoRepositorio.ConsultarDetalheProduto(codProd.Value);
            }

            var viewModel = new ViewProdutoFavoritos()
            {
                produto = produto,
                favoritos = favoritos,
                avaliacaoUnica = avaliacaoUnica

            };

            return View(viewModel);
        }

        public IActionResult CadastrarAvaliacao(decimal codProd, int qtdEstrela, string comentario)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _produtoRepositorio.CadastrarAvaliacao(qtdEstrela, comentario, codLogin.codLogin, codProd);

            TempData["msg"] = mensagem;
            return RedirectToAction("DetalhesProduto", new { codProd });
        }


        public IActionResult DeletarAvaliacao(int codLogin, int codAvaliacao)
        {
            // Chama o método para deletar a avaliação
            _produtoRepositorio.DeletarAvaliacao(codLogin, codAvaliacao);

            // Após a exclusão, redireciona para a página com as avaliações atualizadas
            return RedirectToAction("DetalhesProduto", new { id = codAvaliacao });

        }


        public IActionResult Pesquisa(string pesquisa)
        {
            // Cria uma lista para armazenar vários produtos
            List<Produto> produtos = new List<Produto>();

            // Busca os produtos no banco de dados e armazena na lista
            produtos = _produtoRepositorio.ConsultarProdutoPesquisa(pesquisa).ToList(); // Obtém os produtos

            ViewData["pesquisa"] = pesquisa;
            return View(produtos);
        }
    }
}

