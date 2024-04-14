using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.Entities
{
    public class Rezervacija
    {
        [Key]
        public int RezervacijaId { get; set; }
        public DateOnly DatumDolaska { get; set; }
        public DateOnly DatumOdlaska { get; set; }
        public int Cena {  get; set; }
        public int BrojNocenja { get; set; }
        public int Potvrda { get; set; }
        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        [ForeignKey("Objekat")]
        public int ObjekatId { get; set; }

        public Rezervacija() { }

    }
}
