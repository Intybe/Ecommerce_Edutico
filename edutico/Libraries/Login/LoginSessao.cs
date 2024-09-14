using Newtonsoft.Json;
using edutico.Models;


namespace edutico.Libraries.Login
{
    public class LoginSessao
    {
        //injeção de depencia
        private string Key = "Login.Sessao";
        private Sessao.Sessao _sessao;


        //Construtor
        public LoginSessao(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Models.Login login)
        {
            string loginJSONString = JsonConvert.SerializeObject(login);
            _sessao.Criar("Login.Sessao", loginJSONString);
        }


        public edutico.Models.Login GetLogin()
        {
            if (_sessao.Existe(Key))
            {
                string loginJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<edutico.Models.Login>(loginJSONString); // Retorna o objeto Login
            }
            else
            {
                return null;
            }
        }

        //Remove a sessão e desloga o Cliente
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
