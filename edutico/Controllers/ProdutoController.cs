using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace edutico.Controllers
{
    public class ProdutoController : Controller
    {
        private IProdutoRepositorio? _produtoRepositorio;

        // Construtor com injeção de dependência
        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult CadastroProduto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProduto(string NomeProduto, string CodigoProduto, string DescricaoProduto, string ClassificacaoIndicativa, string HabilidadesSelecionadas, string ValorUnitario, string EstoqueProduto, string CategoriaProduto, List<IFormFile> imgs)
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

            // Chamar o repositório e passar o objeto produto e as imagens
            string resultado = _produtoRepositorio.CadastrarProduto(produto, imgs);

            ViewData["msg"] = resultado;
            return View("CadastroCliente");
        }
    }
}
