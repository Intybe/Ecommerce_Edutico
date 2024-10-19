using edutico.Models;

namespace edutico.Repositorio
{
    public interface IPagamentoRepositorio
    {
        string PagamentoPix(Pagamento pagamento);
        string PagamentoCartao(int NF, int qtdParcela, int codCartao);
    }
}
