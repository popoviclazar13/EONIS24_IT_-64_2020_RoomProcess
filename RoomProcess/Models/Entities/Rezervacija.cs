using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.Entities
{
    public class Rezervacija
    {
        [Key]
        public int RezervacijaId { get; set; }
        public DateTime DatumDolaska { get; set; }
        public DateTime DatumOdlaska { get; set; }
        public int Cena {  get; set; }
        public int BrojNocenja { get; set; }
        public bool Potvrda { get; set; }
        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        [ForeignKey("Objekat")]
        public int? ObjekatId { get; set; }
        public Objekat? Objekat { get; set; }

        public Rezervacija() { }

    }
}
