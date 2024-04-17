using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.Entities
{
    public class Slika
    {
        [Key]
        public int SlikaId { get; set; }
        public string UrlSlike { get; set; }
        [ForeignKey("Objekat")]
        public int ObjekatId { get; set; }
        public Objekat Objekat { get; set; }
    }
}
