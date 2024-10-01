using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace edutico.Controllers
{
    public class ProdutoController : Controller
    {
        private IProdutoRepositorio? _produtoRepositorio;
        private readonly LoginSessao _loginSessao;

        // Construtor com injeção de dependência
        public ProdutoController(IProdutoRepositorio produtoRepositorio, LoginSessao loginSessao)
        {
            _produtoRepositorio = produtoRepositorio;
            _loginSessao = loginSessao;
        }

        public IActionResult CadastroProduto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProduto(string NomeProduto, string CodigoProduto, string DescricaoProduto, string ClassificacaoIndicativa, string HabilidadesSelecionadas, string ValorUnitario, string EstoqueProduto, string CategoriaProduto, List<IFormFile> imgs)
        {
            if (imgs == null || !imgs.Any())
            {
                ViewData["msg"] = "Nenhuma imagem foi enviada!";
                return View("CadastroCliente");
            }
            else
            {
                Produto produto = new Produto()
                {
                    codProd = Convert.ToDecimal(CodigoProduto),
                    nomeProd = Convert.ToString(NomeProduto),
                    descricao = Convert.ToString(DescricaoProduto),
                    classificacao = Convert.ToString(ClassificacaoIndicativa),
                    categoria = Convert.ToString(CategoriaProduto),
                    valorUnit = Convert.ToDecimal(ValorUnitario),
                    estoque = Convert.ToInt32(EstoqueProduto)
                };

                List<string> habilidades = HabilidadesSelecionadas.Split(',').ToList();

                // Chamar o repositório e passar o objeto produto e as imagens
                string resultado = _produtoRepositorio.CadastrarProduto(produto, imgs, habilidades);

                ViewData["msg"] = resultado;
                return View("CadastroProduto");
            }
        }

        public IActionResult DetalhesProduto(decimal? codProd)
        {
            Produto produto = null;

            // Verifica se o codProd é nulo ou não foi passado
            if (!codProd.HasValue)
            {
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
            }
            else
            {
                // Consultar o produto pelo código
                produto = _produtoRepositorio.ConsultarDetalheProduto(codProd.Value);
            }

            return View(produto);
        }
    }
}
