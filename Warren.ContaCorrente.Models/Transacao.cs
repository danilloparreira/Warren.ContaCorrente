namespace Warren.ContaCorrente.Models
{
    public class Transacao
    {
        public int? Id { get; set; }

        public int? CodigoCliente { get; set; }

        public string? Descricao { get; set; }

        public decimal? Saldo { get; set; }

        public DateTime? Data { get; set;}
    }
}
