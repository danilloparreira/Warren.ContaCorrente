using FluentMigrator;
using Warren.ContaCorrente.Models;

namespace Warren.ContaCorrente.Migrations.MigrationsFiles
{
    [Migration(4)]
    public class MigrationAndSeed : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Delete.Table("Deposito");
            Delete.Table("Pagamento");
            Delete.Table("Resgate");
            Delete.Table("Transacao");
            Delete.Table("ContaCorrente");

            CreateTables();
            SeedTables();
        }

        private void CreateTables()
        {
            Create.Table("ContaCorrente")
                .WithColumn("CodigoCliente").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("SaldoFinanceiro").AsDecimal()
                .WithColumn("SaldoD1").AsDecimal()
                .WithColumn("SaldoD2").AsDecimal()
                .WithColumn("SaldoD3").AsDecimal()
                .WithColumn("RendaFixa").AsDecimal()
                .WithColumn("Acoes").AsDecimal()
                .WithColumn("Opcoes").AsDecimal()
                .WithColumn("Tesouro").AsDecimal();

            Create.Table("Transacao")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("CodigoCliente").AsInt32().ForeignKey("FK_Transacao_ContaCorrente", "ContaCorrente", "CodigoCliente")
                .WithColumn("Descricao").AsString(100)
                .WithColumn("Saldo").AsDecimal()
                .WithColumn("Data").AsDateTime2();

            Create.Table("Deposito")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("CodigoConta").AsInt32().ForeignKey("FK_Deposito_ContaCorrente", "ContaCorrente", "CodigoCliente")
                .WithColumn("Valor").AsDecimal()
                .WithColumn("Origem").AsInt32()
                .WithColumn("Data").AsDateTime2();

            Create.Table("Pagamento")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("CodigoConta").AsInt32().ForeignKey("FK_Pagamento_ContaCorrente", "ContaCorrente", "CodigoCliente")
                .WithColumn("Valor").AsDecimal()
                .WithColumn("Origem").AsInt32()
                .WithColumn("Descricao").AsString();

            Create.Table("Resgate")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("CodigoConta").AsInt32().ForeignKey("FK_Resgate_ContaCorrente", "ContaCorrente", "CodigoCliente")
                .WithColumn("Valor").AsDecimal()
                .WithColumn("Origem").AsInt32()
                .WithColumn("Descricao").AsString();
        }

        private void SeedTables()
        {
            var random = new Random();

            for (int i = 0; i <= 9; i++)
            {
                Insert.IntoTable("ContaCorrente").Row(new Models.ContaCorrente()
                {
                    Acoes = (decimal?)random.NextDouble(),
                    CodigoCliente = i,
                    Opcoes = (decimal?)random.NextDouble(),
                    RendaFixa = (decimal?)random.NextDouble(),
                    SaldoD1 = (decimal?)random.NextDouble(),
                    SaldoD2 = (decimal?)random.NextDouble(),
                    SaldoD3 = (decimal?)random.NextDouble(),
                    SaldoFinanceiro = (decimal?)random.NextDouble(),
                    Tesouro = (decimal?)random.NextDouble()
                });
            }
        }
    }
}
