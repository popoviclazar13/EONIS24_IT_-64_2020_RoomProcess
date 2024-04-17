using RoomProcess.Models.Entities;

namespace RoomProcess.Models.DTO
{
    public class ObjekatUpdateDTO
    {
        public int ObjekatId { get; set; }
        public string ObjekatNaziv { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public int Cena { get; set; }
        public int Naknade { get; set; }
        public ICollection<Slika> Slike { get; set; }
        public int KorisnikId { get; set; }
        //public KorisnikDTO korisnikDTO { get; set; }
        public int TipObjektaId { get; set; }
        //public TipObjektaDTO tipObjektaDTO { get; set; }
        public int PopustId { get; set; }
        //public PopustDTO popustDTO { get; set; }
    }
}
