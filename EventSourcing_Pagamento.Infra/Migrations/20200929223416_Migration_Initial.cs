using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventSourcing_Pagamento.Infra.Migrations
{
    public partial class Migration_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeNoCartaoDeCredito = table.Column<string>(nullable: true),
                    NumeroDoCartaoDeCredito = table.Column<string>(nullable: true),
                    IdDoPedido = table.Column<int>(nullable: false),
                    Aprovado = table.Column<bool>(nullable: false),
                    BandeiraDoCartao = table.Column<string>(nullable: true),
                    DataDoPagamento = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
