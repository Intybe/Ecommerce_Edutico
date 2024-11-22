using edutico.Models;

namespace edutico.Repositorio
{
    public interface ILoginRepositorio
    {
        Login Login(string usuario, string senha);
    }


}
