using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RoomProcess.Models.Entities
{
    public class Racun
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Rezervacija")]
        public int RezervacijaID { get; set; }
        public virtual Rezervacija? Rezervacija { get; set; }
        public string RacunID { get; set; }
        public DateTime Datum { get; set; }
        public int UkupnaCena { get; set; }
        public string Status { get; set; }
    }
}
