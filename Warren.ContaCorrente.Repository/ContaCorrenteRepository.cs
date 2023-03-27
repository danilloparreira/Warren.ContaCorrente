using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Warren.ContaCorrente.Models;
using Warren.ContaCorrente.Repository.Abstractions;

namespace Warren.ContaCorrente.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly ILogger<ContaCorrenteRepository> _logger;
        private readonly string _connectionString;

        public ContaCorrenteRepository(ILogger<ContaCorrenteRepository> logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public async Task<List<Transacao>> ObterHistoricoDaContaAsync(int codigo)
        {
            var transacoes = new List<Transacao>();

            try
            {
                var sql = $"SELECT * FROM Transacao where CodigoCliente = {codigo} ORDER BY Data DESC";

                using var connection = new MySqlConnection(_connectionString);
                transacoes = (await connection.QueryAsync<Transacao>(sql)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return transacoes;
        }

        public async Task<List<Transacao>> ObterHistoricoDaContaPorPeriodoAsync(int codigo, DateTime dataInicio, DateTime dataFim)
        {
            var transacoes = new List<Transacao>();

            try
            {
                var sql = $"SELECT * FROM Transacao WHERE CodigoCliente = {codigo} and (CAST(Data AS DATE) BETWEEN '{dataInicio:yyyy-MM-dd}' AND '{dataFim:yyyy-MM-dd}') ORDER BY Data DESC";

                using var connection = new MySqlConnection(_connectionString);
                transacoes = (await connection.QueryAsync<Transacao>(sql)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return transacoes;
        }

        public async Task<Models.ContaCorrente> ObterSaldoAsync(int codigo)
        {
            var saldo = new Models.ContaCorrente();
            try
            {
                var sql = $"SELECT * FROM ContaCorrente where CodigoCliente = {codigo}";

                using var connection = new MySqlConnection(_connectionString);
                saldo = await connection.QueryFirstOrDefaultAsync<Models.ContaCorrente>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return saldo;
        }

        public async Task<Deposito> RealizarDepositoAsync(Deposito deposito)
        {
            var depositoRetorno = new Deposito();

            try
            {
                var sql = $"INSERT INTO Deposito(Id, CodigoConta, Valor, Origem, Data) VALUES(@Id, @CodigoConta, @Valor, @Origem, @Data)";
                deposito.Id = Random.Shared.Next();

                using var connection = new MySqlConnection(_connectionString);
                var entidadesSalvas = await connection.ExecuteAsync(sql, new
                {
                    deposito.Id,
                    deposito.CodigoConta,
                    deposito.Valor,
                    deposito.Origem,
                    deposito.Data,
                });

                if (entidadesSalvas >= 1)
                {
                    depositoRetorno = await ObterDeposito(deposito.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return depositoRetorno;
        }

        private async Task<Deposito> ObterDeposito(int id)
        {
            var deposito = new Deposito();
            try
            {
                var sql = $"SELECT * FROM Deposito where Id = {id}";

                using var connection = new MySqlConnection(_connectionString);
                deposito = await connection.QueryFirstOrDefaultAsync<Deposito>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return deposito;
        }

        public async Task<Pagamento> RealizarPagamentoAsync(Pagamento pagamento)
        {
            var pagamentoRetorno = new Pagamento();

            try
            {
                var sql = $"INSERT INTO Pagamento(Id, CodigoConta, Valor, Origem, Descricao) VALUES(@Id, @CodigoConta, @Valor, @Origem, @Descricao)";
                pagamento.Id = Random.Shared.Next();

                using var connection = new MySqlConnection(_connectionString);
                var entidadesSalvas = await connection.ExecuteAsync(sql, new
                {
                    pagamento.Id,
                    pagamento.CodigoConta,
                    pagamento.Valor,
                    pagamento.Origem,
                    pagamento.Descricao,
                });

                if (entidadesSalvas >= 1)
                {
                    pagamentoRetorno = await ObterPagamento(pagamento.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return pagamentoRetorno;
        }

        private async Task<Pagamento?> ObterPagamento(int id)
        {
            var deposito = new Pagamento();
            try
            {
                var sql = $"SELECT * FROM Pagamento where Id = {id}";

                using var connection = new MySqlConnection(_connectionString);
                deposito = await connection.QueryFirstOrDefaultAsync<Pagamento>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return deposito;
        }

        public async Task<Resgate> RealizarResgateAsync(Resgate resgate)
        {
            var resgateRetorno = new Resgate();

            try
            {
                var sql = $"INSERT INTO Resgate(Id, CodigoConta, Valor, Origem, Descricao) VALUES(@Id, @CodigoConta, @Valor, @Origem, @Descricao)";
                resgate.Id = Random.Shared.Next();

                using var connection = new MySqlConnection(_connectionString);
                var entidadesSalvas = await connection.ExecuteAsync(sql, new
                {
                    resgate.Id,
                    resgate.CodigoConta,
                    resgate.Valor,
                    resgate.Origem,
                    resgate.Descricao,
                });

                if (entidadesSalvas >= 1)
                {
                    resgateRetorno = await ObterResgate(resgate.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return resgateRetorno;
        }

        private async Task<Resgate?> ObterResgate(int id)
        {
            var resgate = new Resgate();
            try
            {
                var sql = $"SELECT * FROM Resgate where Id = {id}";

                using var connection = new MySqlConnection(_connectionString);
                resgate = await connection.QueryFirstOrDefaultAsync<Resgate>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return resgate;
        }

        public async Task<Models.ContaCorrente> AtualizarSaldo(decimal? valor, int? codigoConta)
        {
            var saldo = new Models.ContaCorrente();

            try
            {
                var sql = $"UPDATE ContaCorrente SET SaldoFinanceiro = @Valor WHERE CodigoCliente = @CodigoConta";

                using var connection = new MySqlConnection(_connectionString);
                var entidadesSalvas = await connection.ExecuteAsync(sql, new
                {
                    Valor = valor,
                    CodigoConta = codigoConta
                });

                if (entidadesSalvas >= 1)
                {
                    saldo = await ObterSaldoAsync(codigoConta.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return saldo;
        }

        public async Task<Transacao> InserirTransacao(Transacao transacao)
        {
            var transacaoRetorno = new Transacao();

            try
            {
                var sql = $"INSERT INTO Transacao(Id, CodigoCliente, Saldo, Descricao, Data) VALUES(@Id, @CodigoCliente, @Saldo, @Descricao, @Data)";
                transacao.Id = Random.Shared.Next();

                using var connection = new MySqlConnection(_connectionString);
                var entidadesSalvas = await connection.ExecuteAsync(sql, new
                {
                    transacao.CodigoCliente,
                    transacao.Data,
                    transacao.Descricao,
                    transacao.Id, 
                    transacao.Saldo,
                });

                if (entidadesSalvas >= 1)
                {
                    transacaoRetorno = await ObterTransacao(transacao.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return transacaoRetorno;
        }

        private async Task<Transacao> ObterTransacao(int? id)
        {
            var transacao = new Transacao();

            try
            {
                var sql = $"SELECT * FROM Transacao where Id = {id}";

                using var connection = new MySqlConnection(_connectionString);
                transacao = await connection.QueryFirstOrDefaultAsync<Transacao>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu o seguinte erro: {Message}", ex.Message);
                throw;
            }

            return transacao;
        }
    }
}
