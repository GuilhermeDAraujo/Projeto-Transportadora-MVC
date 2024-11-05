using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_Transportadora_MVC.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoDasTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caminhoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    CustoCombustivel = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustoManutencao = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caminhoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroNotaFiscal = table.Column<int>(type: "int", nullable: false),
                    NomeCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnderecoFaturado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataDoFaturamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroDaCarga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcoesNotaFiscal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoAcao = table.Column<int>(type: "int", nullable: false),
                    DataDaAcao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descriacao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    NotaFiscalId = table.Column<int>(type: "int", nullable: false),
                    CaminhaoId = table.Column<int>(type: "int", nullable: false),
                    DataAgendada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusAgendamento = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcoesNotaFiscal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcoesNotaFiscal_Caminhoes_CaminhaoId",
                        column: x => x.CaminhaoId,
                        principalTable: "Caminhoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcoesNotaFiscal_NotasFiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "NotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fechamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataDoFechamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotaFiscalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fechamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fechamentos_NotasFiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "NotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcoesNotaFiscal_CaminhaoId",
                table: "AcoesNotaFiscal",
                column: "CaminhaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AcoesNotaFiscal_NotaFiscalId",
                table: "AcoesNotaFiscal",
                column: "NotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_Fechamentos_NotaFiscalId",
                table: "Fechamentos",
                column: "NotaFiscalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcoesNotaFiscal");

            migrationBuilder.DropTable(
                name: "Fechamentos");

            migrationBuilder.DropTable(
                name: "Caminhoes");

            migrationBuilder.DropTable(
                name: "NotasFiscais");
        }
    }
}
