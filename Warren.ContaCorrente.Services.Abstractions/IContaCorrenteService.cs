using Warren.ContaCorrente.Models;

namespace Warren.ContaCorrente.Services.Abstractions
{
    public interface IContaCorrenteService
    {
        Task<List<Transacao>> ObterHistoricoDaContaAsync(int codigo);
        Task<List<Transacao>> ObterHistoricoDaContaPorPeriodoAsync(int codigo, DateTime dataInicio, DateTime? dataFim = null);
        Task<Models.ContaCorrente> ObterSaldoAsync(int codigo);
        Task<Deposito> RealizarDepositoAsync(Deposito deposito);
        Task<Pagamento> RealizarPagamentoAsync(Pagamento pagamento);
        Task<Resgate> RealizarResgateAsync(Resgate resgate);
    }
}