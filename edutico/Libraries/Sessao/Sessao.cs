namespace edutico.Libraries.Sessao
{
    public class Sessao
    {

        IHttpContextAccessor _context;
        public Sessao(IHttpContextAccessor context)
        {
            _context = context;
        }

        public void Criar(string Key, string valor)
        {
            _context.HttpContext.Session.SetString(Key, valor);
        }

        //Consultar sessão
        public string Consultar(string Key)
        {
            return _context.HttpContext.Session.GetString(Key);
        }


        public bool Existe(string Key)
        {
            /*  HttpContext- Items pode ser usada para armazenar dados durante o processamento de uma única solicitação.
             * O conteúdo da coleção é descartado após o processamento de uma solicitação*/
            if (_context.HttpContext.Session.GetString(Key) == null)
            {
                return false;
            }

            return true;
        }
        //Remover todas sessão
        public void RemoverTodos()
        {
            _context.HttpContext.Session.Clear();
        }

    }
}
