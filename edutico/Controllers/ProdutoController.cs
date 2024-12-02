using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1;
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


        public IActionResult DetalhesProduto(decimal codProd)
        {
            List<decimal> favoritos = new List<decimal>();

            Produto produto = null;
            Avaliacao avaliacaoUnica = null;


            // Consultar o produto pelo código
            produto = _produtoRepositorio.ConsultarDetalheProduto(codProd);

            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            if (Login != null)
            {

                // Obtém os favoritos do cliente logado
                var favoritosList = _favoritosRepositorio.ConsultarFavoritos(Login.codLogin).ToList();


                // Extrai os códigos dos produtos favoritos
                favoritos = favoritosList.Select(f => f.produto.codProd).ToList();

                avaliacaoUnica = _produtoRepositorio.ConsultarAvaliacaoCliente(codProd, Login.codLogin);
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


        public IActionResult DeletarAvaliacao(int codAvaliacao, decimal codProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            // Chama o método para deletar a avaliação
            string mensagem = _produtoRepositorio.DeletarAvaliacao(Login.codLogin, codAvaliacao, codProd);
            TempData["msg"] = mensagem;

            return RedirectToAction("DetalhesProduto", new { codProd });

        }

        public IActionResult FiltrarClassificacao(string classificacao)
        {
            List<Produto> produtosFiltrados = _produtoRepositorio.ConsultarTodosProdutos()
                    .Where(p => (p.classificacao.codClassificacao.ToString() + " - " + p.classificacao.nomeClassificacao)
                        .Equals(classificacao))
                    .ToList();

            return View("Pesquisa", produtosFiltrados);
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

        [HttpPost]
        public IActionResult FiltrarProdutos(string filtroHabilidade, string filtroCategoria, string filtroClassificacao, string filtroPreco, string pesquisa)
        {
            // Verificando se existe valor e convertendo para uma lista de ints (os Ids)
            var categorias = filtroCategoria?.Split(',').Select(int.Parse).ToList() ?? new List<int>();
            var habilidades = filtroHabilidade?.Split(',').Select(int.Parse).ToList() ?? new List<int>();
            var classificacao = filtroClassificacao?.Split(',').Select(int.Parse).ToList() ?? new List<int>();


            decimal? precoMin = null, precoMax = null;
            if (!string.IsNullOrEmpty(filtroPreco))
            {
                var partes = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(filtroPreco);

                if (decimal.TryParse(partes["min"], out var minValue))
                    precoMin = minValue;

                if (partes["max"]?.ToLower() != "mais" && decimal.TryParse(partes["max"], out var maxValue))
                    precoMax = maxValue;
            }

            List<Produto> listaProdutos = null;

            if (string.IsNullOrEmpty(pesquisa))
            {
                listaProdutos = _produtoRepositorio.ConsultarTodosProdutos().ToList();

            }
            else
            {
                listaProdutos = _produtoRepositorio.ConsultarProdutoPesquisa(pesquisa).ToList();
            }


            // Filtros de categoria
            if (categorias.Any())
                listaProdutos = listaProdutos.Where(p => categorias.Contains(p.categoria.codCategoria)).ToList();

            // Filtros de habilidade
            if (habilidades.Any())
                listaProdutos = listaProdutos.Where(p => habilidades.Any(h => p.habilidades.Select(hb => hb.codHabilidade).Contains(h))).ToList();

            // Filtro de preço
            if (precoMin.HasValue)
                listaProdutos = listaProdutos.Where(p => p.valorUnit >= precoMin.Value).ToList();

            if (precoMax.HasValue)
                listaProdutos = listaProdutos.Where(p => p.valorUnit <= precoMax.Value).ToList();

            // Filtro de classificação
            if (classificacao.Any())
                listaProdutos = listaProdutos.Where(p => classificacao.Contains(p.classificacao.codClassificacao)).ToList();

            // Executa a consulta e obtém os resultados filtrados
            var produtosFiltrados = listaProdutos;

            // Retorna a view com os produtos filtrados
            ViewData["pesquisa"] = pesquisa;
            ViewData["filtroCategoria"] = filtroCategoria;
            ViewData["filtroHabilidade"] = filtroHabilidade;
            ViewData["filtroClassificacao"] = filtroClassificacao;
            ViewData["filtroPreco"] = filtroPreco;
            return View("Pesquisa", produtosFiltrados);
        }
    }
}

