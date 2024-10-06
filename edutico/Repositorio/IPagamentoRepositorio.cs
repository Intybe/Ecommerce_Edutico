namespace edutico.Repositorio
{
    public interface IPagamentoRepositorio
    {
        string PagamentoPix(int NF, int qtdParcela, string codPix);
        string PagamentoCartao(int NF, int qtdParcela, int codCartao);
    }
}
