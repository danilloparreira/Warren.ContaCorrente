using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Warren.ContaCorrente.Models;
using Warren.ContaCorrente.Services.Abstractions;

namespace Warren.ContaCorrente.API.Controllers
{
    /// <summary>
    /// Controller responsável pela conta corrente
    /// </summary>
    [ApiController]
    [Route("v1/conta-corrente")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteService _contaCorrenteService;

        /// <summary>
        /// Construtor da controller
        /// </summary>
        /// <param name="contaCorrenteService"></param>
        public ContaCorrenteController(IContaCorrenteService contaCorrenteService)
        {
            _contaCorrenteService = contaCorrenteService;
        }

        /// <summary>
        /// Endpoint resposável por obter todos os saldos e posição financeira do cliente
        /// </summary>
        /// <param name="codigo">Código da conta corrente do cliente</param>
        /// <returns></returns>
        [HttpGet("saldos/{codigo}", Name = "ObterSaldoAtual")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Models.ContaCorrente))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(NoContentResult))]
        public async Task<IActionResult> ObterSaldoAtualAsync([FromRoute] int codigo)
        {
            var saldo = await _contaCorrenteService.ObterSaldoAsync(codigo);

            if (saldo == null)
            {
                return NotFound();
            }

            return Ok(saldo);
        }

        /// <summary>
        /// Enpoint responsável por obter o histórico de movimentação do cliente pelo código
        /// </summary>
        /// <param name="codigo">Código da conta corrente do cliente</param>
        /// <returns></returns>
        [HttpGet("historicos/{codigo}", Name = "ObterHistoricoDaContaAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Transacao>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(NoContentResult))]
        public async Task<IActionResult> ObterHistoricoDaContaAsync([FromRoute] int codigo)
        {
            var transacoes = await _contaCorrenteService.ObterHistoricoDaContaAsync(codigo);

            if (transacoes == null)
            {
                return NotFound();
            }

            return Ok(transacoes);
        }

        /// <summary>
        /// Enpoint responsável por obter o histórico de movimentação do cliente pelo código e data
        /// </summary>
        /// <param name="codigo">Código da conta corrente do cliente</param>
        /// <param name="dataInicio">Data início</param>
        /// <param name="dataFim">Data final</param>
        /// <returns></returns>
        [HttpGet("historicos-por-periodo/{codigo}/{dataInicio}", Name = "ObterHistoricoDaContaPorPeriodoAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Transacao>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(NoContentResult))]
        public async Task<IActionResult> ObterHistoricoDaContaPorPeriodoAsync([FromRoute] int codigo, DateTime dataInicio, DateTime? dataFim = null)
        {
            var transacoes = await _contaCorrenteService.ObterHistoricoDaContaPorPeriodoAsync(codigo, dataInicio, dataFim);

            if (transacoes == null || transacoes.Count == 0)
            {
                return NotFound();
            }

            return Ok(transacoes);
        }

        /// <summary>
        /// Endpoint responsável por realizar o depósito na conta do cliente
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     {
        ///      "codigoConta": 2,
        ///      "valor": 125.124,
        ///      "origem": 1,
        ///      "data": "2023-03-27T15:57:15.074Z"
        ///     }
        /// 
        /// </remarks>
        /// <param name="deposito">Depósito que será realizado</param>
        /// <returns></returns>
        [HttpPost("depositos", Name = "RealizarDepositoAsync")]
        [ProducesResponseType(typeof(Deposito), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResult))]
        public async Task<IActionResult> RealizarDepositoAsync([FromBody] Deposito deposito)
        {
            var url = Request.GetDisplayUrl();
            var depositoRetorno = await _contaCorrenteService.RealizarDepositoAsync(deposito);

            if (depositoRetorno == null)
            {
                return BadRequest();
            }

            return Created($"{url}/{depositoRetorno.Id}", depositoRetorno);
        }

        /// <summary>
        /// Endpoint responsável por realizar o pagamento na conta do cliente
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     {
        ///      "codigoConta": 2,
        ///      "valor": 12.124,
        ///      "origem": 1,
        ///      "descricao": "Pagamento"
        ///     }
        /// 
        /// </remarks>
        /// <param name="pagamento">Pagamento a ser realizado</param>
        /// <returns></returns>
        [HttpPost("pagamentos", Name = "RealizarPagamentoAsync")]
        [ProducesResponseType(typeof(Pagamento), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResult))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RealizarPagamentoAsync([FromBody] Pagamento pagamento)
        {
            var url = Request.GetDisplayUrl();
            var pagamentoRetorno = await _contaCorrenteService.RealizarPagamentoAsync(pagamento);

            if (pagamentoRetorno == null)
            {
                return BadRequest();
            }

            return Created($"{url}/{pagamentoRetorno.Id}", pagamentoRetorno);
        }

        /// <summary>
        /// Endpoint responsável por realizar o resgate na conta do cliente
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     {
        ///      "codigoConta": 2,
        ///      "valor": 87.3,
        ///      "origem": 1,
        ///      "descricao": "Resgate"
        ///     }
        /// 
        /// </remarks>
        /// <param name="resgate">Resgate a ser realizado</param>
        /// <returns></returns>
        [HttpPost("resgates", Name = "RealizarResgateAsync")]
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResult))]
        public async Task<IActionResult> RealizarResgateAsync([FromBody] Resgate resgate)
        {
            var url = Request.GetDisplayUrl();
            var resgateRetorno = await _contaCorrenteService.RealizarResgateAsync(resgate);

            if (resgateRetorno == null)
            {
                return BadRequest();
            }

            return Created($"{url}/{resgateRetorno.Id}", resgateRetorno);
        }
    }
}