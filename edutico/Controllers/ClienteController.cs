using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Security.Claims;

namespace edutico.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private ICarrinhoRepositorio? _carrinhoRepositorio;
        private IFavoritosRepositorio? _favoritosRepositorio;
        private readonly LoginSessao _loginSessao;

        // Construtor com injeção de dependência
        public ClienteController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao, ICarrinhoRepositorio carrinhoRepositorio, IFavoritosRepositorio favoritosRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao; // Inicializa a variável _loginSessao
            _carrinhoRepositorio = carrinhoRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
        }

        [HttpPost]
        public IActionResult CadastroCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                // Algo deu errado, retorne a View com os erros
                return View(cliente);
            }
            return RedirectToAction("CadastroCliente");
        }

        public IActionResult MeuPerfil()
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            // Chama o método do repositório para consultar o cliente no banco de dados
            Cliente cliente = _clienteRepositorio.ConsultarCliente(codLogin.codLogin);

            // Passa os dados do cliente para a view
            return View("MeuPerfil", cliente);
        }

        [HttpPost]
        public IActionResult CadastrarCliente(decimal cpf, string nome, string sobrenome, string telefone, string email, string logradouro, string bairro, string cidade, string uf, string cep, int numEnd, string compEnd, string senha)
        {
            string mensagem = _clienteRepositorio.CadastrarCliente(cpf, nome, sobrenome, telefone, email, logradouro, bairro, cidade, uf, cep, numEnd, compEnd, senha);

            ViewData["msg"] = mensagem;
            return View("CadastroCliente");
        }

        [HttpPost]
        public IActionResult AtualizarClienteConta(string email, string senha)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            string mensagem = _clienteRepositorio.AtualizarClienteConta(codLogin.codLogin, email, senha);

            ViewData["msg"] = mensagem;

            return RedirectToAction("MeuPerfil");
        }

        [HttpPost]
        public IActionResult AtualizarClienteDados(string cpf, string nome, string sobrenome, string telefone)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _clienteRepositorio.AtualizarClienteDados(codLogin.codLogin, Convert.ToInt64(cpf), nome, sobrenome, telefone);

            ViewData["msg"] = mensagem;

            return RedirectToAction("MeuPerfil");
        }

        [HttpPost]
        public IActionResult AtualizarClienteEndereco(string logradouro, string bairro, string cep, string cidade, string uf, string numEnd, string compEnd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            string mensagem = _clienteRepositorio.AtualizarClienteEndereco(codLogin.codLogin, logradouro, bairro, cep, cidade, uf, Convert.ToInt32(numEnd), compEnd);

            ViewData["msg"] = mensagem;

            return RedirectToAction("MeuPerfil");
        }

        public IActionResult Carrinho()
        {
            // Cria uma lista para armazenar vários produtos
            List<Carrinho> itemCarrinho = new List<Carrinho>();

            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }
            else if (codLogin.nivelAcesso == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    // Primeiro, instancie o produto fora do Carrinho
                    Produto produtos = new Produto
                    {
                        nomeProd = "Nome Produto",  // Exemplo para nome diferente
                        valorUnit = 0,
                        imgs = new List<Imagem> { new Imagem { enderecoImg = "~/imgs/img_prod_padrao_quadrada.png" } }
                    };

                    // Agora, instancie o Carrinho com o produto
                    Carrinho carrinho = new Carrinho
                    {
                        produto = produtos,  // Atribua o produto ao carrinho
                        qtdProd = 0
                    };

                    // Adiciona o carrinho à lista de itens no carrinho
                    itemCarrinho.Add(carrinho);

                }

                return View("Carrinho", itemCarrinho);
            }
            else
            {
                IEnumerable<Carrinho> carrinho = _carrinhoRepositorio.ConsultarCarrinho(codLogin.codLogin);

                return View("Carrinho", carrinho);
            }
        }

        public IActionResult CadastrarProdutoCarrinho(decimal codProd, int qtdProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _carrinhoRepositorio.CadastrarProdutoCarrinho(codLogin.codLogin, codProd, qtdProd);

            ViewData["msg"] = mensagem;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ExcluirItemProduto(decimal codProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _carrinhoRepositorio.ExcluirItemCarrinho(codLogin.codLogin, codProd);

            ViewData["msg"] = mensagem;
            return RedirectToAction("Carrinho", "Cliente");
        }


        [HttpPost]
        public IActionResult AtualizarQtdProdCarrinho(decimal codProd, int qtdProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            // Atualiza a quantidade de produtos no banco de dados
            string mensagem = _carrinhoRepositorio.AtualizarQtdProdCarrinho(codLogin.codLogin, codProd, qtdProd);

            // Exibe uma mensagem de sucesso ou erro
            ViewData["msg"] = mensagem;

            // Redireciona de volta para a página do carrinho
            return RedirectToAction("Carrinho");
        }

        public IActionResult FavoritosCheio()
        {
            // Cria uma lista para armazenar vários produtos
            List<Favoritos> produtoFavoritado = new List<Favoritos>();

            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }
            else
            {
                IEnumerable<Favoritos> favoritos = _favoritosRepositorio.ConsultarFavoritos(codLogin.codLogin);

                return View("FavoritosCheio", favoritos);
            }

        }


    }

}
