using Microsoft.EntityFrameworkCore;
using MedicoPrenotazioni.Models;

namespace MedicoPrenotazioni.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Operatore> Operatori { get; set; }
        public DbSet<Servizio> Servizi { get; set; }
        public DbSet<SlotDisponibilita> SlotDisponibilita { get; set; }
        public DbSet<Prenotazione> Prenotazioni { get; set; }

        public DbSet<NonDisponibile> NonDisponibile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione delle relazioni e vincoli
            
            // Operatore
            modelBuilder.Entity<Operatore>()
                .HasMany(o => o.SlotDisponibilita)
                .WithOne(s => s.Operatore)
                .HasForeignKey(s => s.OperatoreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Servizio
            modelBuilder.Entity<Servizio>()
                .HasMany(s => s.SlotDisponibilita)
                .WithOne(s => s.Servizio)
                .HasForeignKey(s => s.ServizioId)
                .OnDelete(DeleteBehavior.Cascade);

            // SlotDisponibilita
            modelBuilder.Entity<SlotDisponibilita>()
                .HasIndex(s => new { s.OperatoreId, s.ServizioId, s.Giorno })
                .IsUnique(false);

            // Prenotazione
            modelBuilder.Entity<Prenotazione>()
                .HasOne<Operatore>()
                .WithMany()
                .HasForeignKey(p => p.OperatoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prenotazione>()
                .HasOne<Servizio>()
                .WithMany()
                .HasForeignKey(p => p.ServizioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Dati iniziali
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Operatori
            modelBuilder.Entity<Operatore>().HasData(
                new Operatore
                {
                    Id = 1,
                    Nome = "Mario",
                    Cognome = "Rossi",
                    Specializzazione = "Cardiologia",
                    Email = "mario.rossi@esempio.it",
                    Telefono = "3331234567",
                    Note = "Primario del reparto di cardiologia",
                    Attivo = true
                },
                new Operatore
                {
                    Id = 2,
                    Nome = "Laura",
                    Cognome = "Bianchi",
                    Specializzazione = "Dermatologia",
                    Email = "laura.bianchi@esempio.it",
                    Telefono = "3339876543",
                    Note = "Specialista in dermatologia pediatrica",
                    Attivo = true
                },
                new Operatore
                {
                    Id = 3,
                    Nome = "Giuseppe",
                    Cognome = "Verdi",
                    Specializzazione = "Ortopedia",
                    Email = "giuseppe.verdi@esempio.it",
                    Telefono = "3351234567",
                    Note = "Specialista in traumatologia sportiva",
                    Attivo = true
                }
            );

            // Seed Servizi
            modelBuilder.Entity<Servizio>().HasData(
                new Servizio
                {
                    Id = 1,
                    Nome = "Visita Cardiologica",
                    Descrizione = "Visita specialistica con elettrocardiogramma",
                    Prezzo = 120.00m,
                    DurataDefaultMinuti = 30,
                    Note = "Portare eventuali esami precedenti",
                    Attivo = true
                },
                new Servizio
                {
                    Id = 2,
                    Nome = "Visita Dermatologica",
                    Descrizione = "Controllo nei e problemi della pelle",
                    Prezzo = 100.00m,
                    DurataDefaultMinuti = 30,
                    Note = "Evitare creme o trucchi prima della visita",
                    Attivo = true
                },
                new Servizio
                {
                    Id = 3,
                    Nome = "Visita Ortopedica",
                    Descrizione = "Valutazione problemi articolari e muscolari",
                    Prezzo = 110.00m,
                    DurataDefaultMinuti = 45,
                    Note = "Portare eventuali radiografie o risonanze",
                    Attivo = true
                }
            );

            // Seed SlotDisponibilita
            modelBuilder.Entity<SlotDisponibilita>().HasData(
                new SlotDisponibilita
                {
                    Id = 1,
                    OperatoreId = 1,
                    ServizioId = 1,
                    Giorno = GiornoSettimana.Lunedi,
                    OraInizio = new TimeSpan(9, 0, 0),
                    OraFine = new TimeSpan(12, 0, 0),
                    DurataMinuti = 30,
                    Note = "Solo su appuntamento",
                    Attivo = true
                },
                new SlotDisponibilita
                {
                    Id = 2,
                    OperatoreId = 1,
                    ServizioId = 1,
                    Giorno = GiornoSettimana.Mercoledi,
                    OraInizio = new TimeSpan(9, 0, 0),
                    OraFine = new TimeSpan(12, 0, 0),
                    DurataMinuti = 30,
                    Note = "Solo su appuntamento",
                    Attivo = true
                },
                new SlotDisponibilita
                {
                    Id = 3,
                    OperatoreId = 2,
                    ServizioId = 2,
                    Giorno = GiornoSettimana.Martedi,
                    OraInizio = new TimeSpan(14, 0, 0),
                    OraFine = new TimeSpan(18, 0, 0),
                    DurataMinuti = 30,
                    Note = "Disponibile anche per urgenze",
                    Attivo = true
                },
                new SlotDisponibilita
                {
                    Id = 4,
                    OperatoreId = 2,
                    ServizioId = 2,
                    Giorno = GiornoSettimana.Giovedi,
                    OraInizio = new TimeSpan(14, 0, 0),
                    OraFine = new TimeSpan(18, 0, 0),
                    DurataMinuti = 30,
                    Note = "Disponibile anche per urgenze",
                    Attivo = true
                },
                new SlotDisponibilita
                {
                    Id = 5,
                    OperatoreId = 3,
                    ServizioId = 3,
                    Giorno = GiornoSettimana.Venerdi,
                    OraInizio = new TimeSpan(9, 0, 0),
                    OraFine = new TimeSpan(18, 0, 0),
                    DurataMinuti = 45,
                    Note = "Pausa pranzo 13:00-14:00",
                    Attivo = true
                }
            );
        }
    }
}
