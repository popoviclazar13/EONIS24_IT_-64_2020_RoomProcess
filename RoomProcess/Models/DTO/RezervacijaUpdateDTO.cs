using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.DTO
{
    public class RezervacijaUpdateDTO
    {
        public int RezervacijaId { get; set; }
        public DateOnly DatumDolaska { get; set; }
        public DateOnly DatumOdlaska { get; set; }
        public int Cena { get; set; }
        public int BrojNocenja { get; set; }
        public int Potvrda { get; set; }
        public int KorisnikId { get; set; }
        public int ObjekatId { get; set; }
    }
}
