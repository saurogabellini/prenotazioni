using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicoPrenotazioni.Models
{
    public class Servizio
    {
        public Servizio()
        {
            SlotDisponibilita = new List<SlotDisponibilita>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome del servizio è obbligatorio")]
        [StringLength(200, ErrorMessage = "Il nome del servizio non può superare i 200 caratteri")]
        public string Nome { get; set; }

        [StringLength(1000, ErrorMessage = "La descrizione non può superare i 1000 caratteri")]
        public string Descrizione { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un valore positivo")]
        public decimal Prezzo { get; set; }

        [Range(10, 240, ErrorMessage = "La durata deve essere compresa tra 10 e 240 minuti")]
        public int DurataDefaultMinuti { get; set; } = 30;

        public bool Attivo { get; set; } = true;

        [StringLength(500, ErrorMessage = "Le note non possono superare i 500 caratteri")]
        public string Note { get; set; }

        // Proprietà di navigazione per la relazione con SlotDisponibilita
        public virtual ICollection<SlotDisponibilita> SlotDisponibilita { get; set; }
    }
}
