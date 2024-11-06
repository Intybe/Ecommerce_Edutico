using edutico.Models;

namespace edutico.Repositorio
{
    public interface IFavoritosRepositorio
    {
        public void Favoritar(int codLogin, decimal codProd);
        public void RemoverFavorito(int codLogin, decimal codProd);
        IEnumerable<Favorito> ConsultarFavoritos(int codLogin);
    }
}
