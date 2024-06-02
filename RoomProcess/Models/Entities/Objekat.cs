using RoomProcess.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.Entities
{
    public class Objekat
    {
        [Key]
        public int ObjekatId { get; set; }
        public string ObjekatNaziv {  get; set; }
        public string Adresa { get; set; }
        public string Grad {  get; set; }
        public int Cena { get; set; }
        public int Naknade { get; set; }
        public int x {  get; set; }
        public int y { get; set; }

        //public ICollection<SlikaDTO> Slike { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        [ForeignKey("TipObjekta")]
        public int TipObjektaId { get; set; }
        public TipObjekta TipObjekta { get; set; }
        [ForeignKey("Popust")]
        public int PopustId { get; set; }
        public Popust Popust { get; set; }

        public Objekat() { }

    }
}
