using Warren.ContaCorrente.Models;
using Warren.ContaCorrente.Repository.Abstractions;
using Warren.ContaCorrente.Services.Abstractions;

namespace Warren.ContaCorrente.Services
{
    public class ContaCorrenteService : IContaCorrenteService
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public ContaCorrenteService(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<List<Transacao>> ObterHistoricoDaContaAsync(int codigo)
        {
            return await _contaCorrenteRepository.ObterHistoricoDaContaAsync(codigo);
        }

        public async Task<List<Transacao>> ObterHistoricoDaContaPorPeriodoAsync(int codigo, DateTime dataInicio, DateTime? dataFim = null)
        {
            if (!dataFim.HasValue)
            {
                dataFim = DateTime.UtcNow;
            }

            return await _contaCorrenteRepository.ObterHistoricoDaContaPorPeriodoAsync(codigo, dataInicio, dataFim.Value);
        }

        public async Task<Models.ContaCorrente> ObterSaldoAsync(int codigo)
        {
            return await _contaCorrenteRepository.ObterSaldoAsync(codigo);
        }

        public async Task<Deposito> RealizarDepositoAsync(Deposito deposito)
        {
            var saldo = await AtualizarSaldo(Math.Abs(deposito.Valor.Value), deposito.CodigoConta);
            await InserirTransacao(saldo.SaldoFinanceiro, deposito.CodigoConta, "Depósito");
            return await _contaCorrenteRepository.RealizarDepositoAsync(deposito);
        }

        private async Task InserirTransacao(decimal? saldoFinanceiro, int? codigoConta, string descricao)
        {
            var transacao = new Transacao()
            {
                CodigoCliente = codigoConta,
                Data = DateTime.Now,
                Descricao = descricao,
                Id = 0,
                Saldo = saldoFinanceiro,
            };

            await _contaCorrenteRepository.InserirTransacao(transacao);
        }

        private async Task<Models.ContaCorrente> AtualizarSaldo(decimal? valor, int? codigoConta)
        {
            var saldoAtual = (await _contaCorrenteRepository.ObterSaldoAsync(codigoConta.Value)).SaldoFinanceiro;
            return await _contaCorrenteRepository.AtualizarSaldo(saldoAtual + valor, codigoConta);
        }

        public async Task<Pagamento> RealizarPagamentoAsync(Pagamento pagamento)
        {
            var saldo = await AtualizarSaldo(Math.Abs(pagamento.Valor.Value), pagamento.CodigoConta);
            await InserirTransacao(saldo.SaldoFinanceiro, pagamento.CodigoConta, "Pagamento");
            return await _contaCorrenteRepository.RealizarPagamentoAsync(pagamento);
        }

        public async Task<Resgate> RealizarResgateAsync(Resgate resgate)
        {
            var saldo = await AtualizarSaldo(-Math.Abs(resgate.Valor.Value), resgate.CodigoConta);
            await InserirTransacao(saldo.SaldoFinanceiro, resgate.CodigoConta, "Resgate");
            return await _contaCorrenteRepository.RealizarResgateAsync(resgate);
        }
    }
}
