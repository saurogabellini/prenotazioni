using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicoPrenotazioni.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operatori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Specializzazione = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Attivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operatori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servizi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Prezzo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurataDefaultMinuti = table.Column<int>(type: "int", nullable: false),
                    Attivo = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servizi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prenotazioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatoreId = table.Column<int>(type: "int", nullable: false),
                    OperatoreId1 = table.Column<int>(type: "int", nullable: false),
                    ServizioId = table.Column<int>(type: "int", nullable: false),
                    ServizioId1 = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OraInizio = table.Column<TimeSpan>(type: "time", nullable: false),
                    OraFine = table.Column<TimeSpan>(type: "time", nullable: false),
                    NomeCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CognomeCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TelefonoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stato = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataModifica = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prenotazioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prenotazioni_Operatori_OperatoreId",
                        column: x => x.OperatoreId,
                        principalTable: "Operatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prenotazioni_Operatori_OperatoreId1",
                        column: x => x.OperatoreId1,
                        principalTable: "Operatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prenotazioni_Servizi_ServizioId",
                        column: x => x.ServizioId,
                        principalTable: "Servizi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prenotazioni_Servizi_ServizioId1",
                        column: x => x.ServizioId1,
                        principalTable: "Servizi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlotDisponibilita",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatoreId = table.Column<int>(type: "int", nullable: false),
                    ServizioId = table.Column<int>(type: "int", nullable: false),
                    Giorno = table.Column<int>(type: "int", nullable: false),
                    OraInizio = table.Column<TimeSpan>(type: "time", nullable: false),
                    OraFine = table.Column<TimeSpan>(type: "time", nullable: false),
                    DurataMinuti = table.Column<int>(type: "int", nullable: false),
                    Attivo = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotDisponibilita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlotDisponibilita_Operatori_OperatoreId",
                        column: x => x.OperatoreId,
                        principalTable: "Operatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SlotDisponibilita_Servizi_ServizioId",
                        column: x => x.ServizioId,
                        principalTable: "Servizi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Operatori",
                columns: new[] { "Id", "Attivo", "Cognome", "Email", "Nome", "Note", "Specializzazione", "Telefono" },
                values: new object[,]
                {
                    { 1, true, "Rossi", "mario.rossi@esempio.it", "Mario", "Primario del reparto di cardiologia", "Cardiologia", "3331234567" },
                    { 2, true, "Bianchi", "laura.bianchi@esempio.it", "Laura", "Specialista in dermatologia pediatrica", "Dermatologia", "3339876543" },
                    { 3, true, "Verdi", "giuseppe.verdi@esempio.it", "Giuseppe", "Specialista in traumatologia sportiva", "Ortopedia", "3351234567" }
                });

            migrationBuilder.InsertData(
                table: "Servizi",
                columns: new[] { "Id", "Attivo", "Descrizione", "DurataDefaultMinuti", "Nome", "Note", "Prezzo" },
                values: new object[,]
                {
                    { 1, true, "Visita specialistica con elettrocardiogramma", 30, "Visita Cardiologica", "Portare eventuali esami precedenti", 120.00m },
                    { 2, true, "Controllo nei e problemi della pelle", 30, "Visita Dermatologica", "Evitare creme o trucchi prima della visita", 100.00m },
                    { 3, true, "Valutazione problemi articolari e muscolari", 45, "Visita Ortopedica", "Portare eventuali radiografie o risonanze", 110.00m }
                });

            migrationBuilder.InsertData(
                table: "SlotDisponibilita",
                columns: new[] { "Id", "Attivo", "DurataMinuti", "Giorno", "Note", "OperatoreId", "OraFine", "OraInizio", "ServizioId" },
                values: new object[,]
                {
                    { 1, true, 30, 1, "Solo su appuntamento", 1, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 1 },
                    { 2, true, 30, 3, "Solo su appuntamento", 1, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 1 },
                    { 3, true, 30, 2, "Disponibile anche per urgenze", 2, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 14, 0, 0, 0), 2 },
                    { 4, true, 30, 4, "Disponibile anche per urgenze", 2, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 14, 0, 0, 0), 2 },
                    { 5, true, 45, 5, "Pausa pranzo 13:00-14:00", 3, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prenotazioni_OperatoreId",
                table: "Prenotazioni",
                column: "OperatoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Prenotazioni_OperatoreId1",
                table: "Prenotazioni",
                column: "OperatoreId1");

            migrationBuilder.CreateIndex(
                name: "IX_Prenotazioni_ServizioId",
                table: "Prenotazioni",
                column: "ServizioId");

            migrationBuilder.CreateIndex(
                name: "IX_Prenotazioni_ServizioId1",
                table: "Prenotazioni",
                column: "ServizioId1");

            migrationBuilder.CreateIndex(
                name: "IX_SlotDisponibilita_OperatoreId_ServizioId_Giorno",
                table: "SlotDisponibilita",
                columns: new[] { "OperatoreId", "ServizioId", "Giorno" });

            migrationBuilder.CreateIndex(
                name: "IX_SlotDisponibilita_ServizioId",
                table: "SlotDisponibilita",
                column: "ServizioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prenotazioni");

            migrationBuilder.DropTable(
                name: "SlotDisponibilita");

            migrationBuilder.DropTable(
                name: "Operatori");

            migrationBuilder.DropTable(
                name: "Servizi");
        }
    }
}
