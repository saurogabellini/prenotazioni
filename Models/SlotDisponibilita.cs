using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicoPrenotazioni.Models
{
    public enum GiornoSettimana
    {
        Lunedi = 1,
        Martedi = 2,
        Mercoledi = 3,
        Giovedi = 4,
        Venerdi = 5,
        Sabato = 6,
        Domenica = 7
    }

    public class SlotDisponibilita
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

        [Required(ErrorMessage = "Il giorno della settimana è obbligatorio")]
        public GiornoSettimana Giorno { get; set; }

        [Required(ErrorMessage = "L'ora di inizio è obbligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan OraInizio { get; set; }

        [Required(ErrorMessage = "L'ora di fine è obbligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan OraFine { get; set; }

        [Range(10, 240, ErrorMessage = "La durata deve essere compresa tra 10 e 240 minuti")]
        public int DurataMinuti { get; set; }

        public bool Attivo { get; set; } = true;

        [StringLength(500, ErrorMessage = "Le note non possono superare i 500 caratteri")]
        public string Note { get; set; }

        // Metodo per verificare se lo slot è valido
        public bool IsValido()
        {
            return OraInizio < OraFine && DurataMinuti > 0;
        }

        // Metodo per calcolare il numero di appuntamenti possibili in questo slot
        public int CalcolaNumeroAppuntamentiPossibili()
        {
            int minutiTotali = (int)(OraFine - OraInizio).TotalMinutes;
            return minutiTotali / DurataMinuti;
        }
    }
}
