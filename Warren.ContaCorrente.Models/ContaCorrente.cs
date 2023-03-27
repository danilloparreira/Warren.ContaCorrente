namespace Warren.ContaCorrente.Models
{
    public class ContaCorrente
    {
        /// <summary>
        /// Código de identificação da conta corrente
        /// </summary>
        public int? CodigoCliente { get; set; }

        /// <summary>
        /// Saldo financeiro da conta corrente
        /// </summary>
        public decimal? SaldoFinanceiro { get; set; }

        /// <summary>
        /// Saldo financeiro da conta corrente em D+1
        /// </summary>
        public decimal? SaldoD1 { get; set; }

        /// <summary>
        /// Saldo financeiro da conta corrente em D+2
        /// </summary>
        public decimal? SaldoD2 { get; set; }

        /// <summary>
        /// Saldo financeiro da conta corrente em D+3
        /// </summary>
        public decimal? SaldoD3 { get; set; }

        /// <summary>
        /// Somatório da posição financeira do cliente em renda fixa
        /// </summary>
        public decimal? RendaFixa { get; set; }

        /// <summary>
        /// Somatório da posição financeira do cliente em ações
        /// </summary>
        public decimal? Acoes { get; set; }

        /// <summary>
        /// Somatório da posição financeira do cliente em opções
        /// </summary>
        public decimal? Opcoes { get; set; }

        /// <summary>
        /// Somatório da posição financeira do cliente em tesouro direto
        /// </summary>
        public decimal? Tesouro { get; set; }
    }
}