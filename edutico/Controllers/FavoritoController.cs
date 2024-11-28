using Microsoft.AspNetCore.Mvc;
using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;

namespace edutico.Controllers
{
    public class FavoritoController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private IFavoritosRepositorio? _favoritosRepositorio;
        private readonly LoginSessao _loginSessao;


        // Construtor com injeção de dependência
        public FavoritoController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao, IFavoritosRepositorio favoritosRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao;
            _favoritosRepositorio = favoritosRepositorio;
        }


        // Método para consultar os Produtos favoritados do cliente e abrir a tela "MeusFavoritos"
        public IActionResult Meusfavoritos()
        {
            // Pega o Login do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            if (Login == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }
            else
            {
                IEnumerable<Favorito> favoritos = _favoritosRepositorio.ConsultarFavoritos(Login.codLogin);

                return View("MeusFavoritos", favoritos);
            }
        }


        // Método para favoritar algum produto
        [HttpPost]
        public IActionResult Favoritar(decimal codProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            if (Login == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            _favoritosRepositorio.Favoritar(Login.codLogin, codProd);

            return Redirect(Request.Headers["Referer"].ToString());
        }


        // Método para remover algum produto dos favoritos
        [HttpPost]
        public IActionResult RemoverFavorito(decimal codProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            _favoritosRepositorio.RemoverFavorito(codLogin.codLogin, codProd);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
