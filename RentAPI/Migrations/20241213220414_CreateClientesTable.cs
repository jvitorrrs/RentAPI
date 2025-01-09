using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    ValorContrato = table.Column<int>(type: "int", nullable: false),
                    InicioContrato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminoContrato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaVencimento = table.Column<int>(type: "int", nullable: false),
                    UltimoPgto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
