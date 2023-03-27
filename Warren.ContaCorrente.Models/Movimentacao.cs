using Warren.ContaCorrente.Models.Enums;

namespace Warren.ContaCorrente.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }

        public int? CodigoConta { get; set; }

        public decimal? Valor { get; set; }

        public Origem? Origem { get; set; }
    }
}
