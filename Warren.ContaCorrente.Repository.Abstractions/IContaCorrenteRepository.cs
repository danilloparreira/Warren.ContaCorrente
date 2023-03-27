using Warren.ContaCorrente.Models;

namespace Warren.ContaCorrente.Repository.Abstractions
{
    public interface IContaCorrenteRepository
    {
        Task<Models.ContaCorrente> AtualizarSaldo(decimal? valor, int? codigoConta);
        Task<Transacao> InserirTransacao(Transacao transacao);
        Task<List<Transacao>> ObterHistoricoDaContaAsync(int codigo);
        Task<List<Transacao>> ObterHistoricoDaContaPorPeriodoAsync(int codigo, DateTime dataInicio, DateTime dataFim);
        Task<Models.ContaCorrente> ObterSaldoAsync(int codigo);
        Task<Deposito> RealizarDepositoAsync(Deposito deposito);
        Task<Pagamento> RealizarPagamentoAsync(Pagamento pagamento);
        Task<Resgate> RealizarResgateAsync(Resgate resgate);
    }
}