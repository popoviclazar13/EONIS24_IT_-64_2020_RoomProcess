using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.DTO
{
    public class RecenzijaUpdateDTO
    {
        public int RecenzijaId { get; set; }
        public string Tekst { get; set; }
        public DateTime Datum { get; set; }
        public int Lokacija { get; set; }
        public int Cistoca { get; set; }
        public int Osoblje { get; set; }
        public int Sadrzaj { get; set; }
        public int CenaKvalitet { get; set; } // ODNOS CENE I KVALITETA
        public int Ocena { get; set; }
        public int KorisnikId { get; set; }
        public int RezervacijaId { get; set; }
    }
}
