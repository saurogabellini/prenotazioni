using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicoPrenotazioni.Migrations
{
    /// <inheritdoc />
    public partial class NonDisponibilita2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NonDisponibile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatoreId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OraInizio = table.Column<TimeSpan>(type: "time", nullable: false),
                    OraFine = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonDisponibile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonDisponibile_Operatori_OperatoreId",
                        column: x => x.OperatoreId,
                        principalTable: "Operatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NonDisponibile_OperatoreId",
                table: "NonDisponibile",
                column: "OperatoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NonDisponibile");
        }
    }
}
