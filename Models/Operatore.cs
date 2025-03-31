using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicoPrenotazioni.Models
{
    public class Operatore
    {
        public Operatore()
        {
            SlotDisponibilita = new List<SlotDisponibilita>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il cognome non può superare i 100 caratteri")]
        public string Cognome { get; set; }

        [StringLength(200, ErrorMessage = "La specializzazione non può superare i 200 caratteri")]
        public string Specializzazione { get; set; }

        [EmailAddress(ErrorMessage = "Formato email non valido")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Formato telefono non valido")]
        public string Telefono { get; set; }

        [StringLength(500, ErrorMessage = "La nota non può superare i 500 caratteri")]
        public string Note { get; set; }

        public bool Attivo { get; set; } = true;

        // Proprietà calcolata per visualizzare il nome completo
        public string NomeCompleto => $"{Nome} {Cognome}";

        // Proprietà di navigazione per la relazione con SlotDisponibilita
        public virtual ICollection<SlotDisponibilita> SlotDisponibilita { get; set; }
    }
}
