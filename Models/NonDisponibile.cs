using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MedicoPrenotazioni.Models
{
    public class NonDisponibile
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "L'operatore è obbligatorio")]
        public int OperatoreId { get; set; }

        [ForeignKey("OperatoreId")]
        public Operatore Operatore { get; set; }


        [Required(ErrorMessage = "La data è obbligatoria")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "L'ora di inizio è obbligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan OraInizio { get; set; }

        [Required(ErrorMessage = "L'ora di fine è obbligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan OraFine { get; set; }

    }
}
