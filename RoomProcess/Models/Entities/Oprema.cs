using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.Entities
{
    public class Oprema
    {
        [Key]
        public int OpremaId { get; set; }
        public string OpremaNaziv {  get; set; }
        [ForeignKey("Objekat")]
        public int ObjekatId { get; set; }

        public Oprema()
        {

        }
    }
}
