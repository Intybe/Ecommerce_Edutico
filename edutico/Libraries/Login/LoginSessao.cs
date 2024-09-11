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
       


        public LoginSessao GetCliente()
        {
            /* Deserializar-Já a desserialização permite que os 
            objetos persistidos em arquivos possam ser recuperados e seus valores recriados na memória*/

            if (_sessao.Existe(Key))
            {
                string loginJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<LoginSessao>(loginJSONString);
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
