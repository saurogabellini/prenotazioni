using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicoPrenotazioni.Models
{
    public enum StatoPrenotazione
    {
        Prenotata,
        Confermata,
        Completata,
        Annullata
    }

    public class Prenotazione
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'operatore è obbligatorio")]
        public int OperatoreId { get; set; }

        [ForeignKey("OperatoreId")]
        public Operatore Operatore { get; set; }

        [Required(ErrorMessage = "Il servizio è obbligatorio")]
        public int ServizioId { get; set; }

        [ForeignKey("ServizioId")]
        public Servizio Servizio { get; set; }

        [Required(ErrorMessage = "La data è obbligatoria")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "L'ora di inizio è obbligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan OraInizio { get; set; }

        [Required(ErrorMessage = "L'ora di fine è obbligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan OraFine { get; set; }

        [Required(ErrorMessage = "Il nome del cliente è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome del cliente non può superare i 100 caratteri")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "Il cognome del cliente è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il cognome del cliente non può superare i 100 caratteri")]
        public string CognomeCliente { get; set; }

        [Required(ErrorMessage = "Il telefono del cliente è obbligatorio")]
        [Phone(ErrorMessage = "Formato telefono non valido")]
        public string TelefonoCliente { get; set; }

        [EmailAddress(ErrorMessage = "Formato email non valido")]
        public string EmailCliente { get; set; }

        [Required(ErrorMessage = "Lo stato della prenotazione è obbligatorio")]
        public StatoPrenotazione Stato { get; set; } = StatoPrenotazione.Prenotata;

        [StringLength(1000, ErrorMessage = "Le note non possono superare i 1000 caratteri")]
        public string Note { get; set; }

        public DateTime DataCreazione { get; set; } = DateTime.Now;

        public DateTime? DataModifica { get; set; }

        // Proprietà calcolata per visualizzare il nome completo del cliente
        public string NomeCompletoCliente => $"{NomeCliente} {CognomeCliente}";
    }
}
