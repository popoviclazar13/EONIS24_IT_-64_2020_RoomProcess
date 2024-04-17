using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.Entities
{
    public class Recenzija
    {
        [Key]
        public int RecenzijaId { get; set; }
        public string Tekst { get; set; }
        public DateTime Datum {  get; set; }
        public int Lokacija { get; set; }
        public int Cistoca { get; set; }
        public int Osoblje { get; set; }
        public int Sadrzaj {  get; set; }
        public int CenaKvalitet { get; set; } // ODNOS CENE I KVALITETA
        public int Ocena {  get; set; }
        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        [ForeignKey("Rezervacija")]
        public int? RezervacijaId { get; set; }
        public Rezervacija? Rezervacija { get; set; }

        public Recenzija() { }
    }
}
