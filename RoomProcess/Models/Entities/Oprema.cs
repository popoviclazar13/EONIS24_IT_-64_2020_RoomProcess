using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.Entities
{
    public class Oprema
    {
        public Oprema()
        {
            //Objekats = new List<Objekat>();
        }

        [Key]
        public int OpremaId { get; set; }
        public string OpremaNaziv {  get; set; }
        [ForeignKey("Objekat")]
        public int ObjekatId { get; set; }
        public Objekat Objekat { get; set; }

        //public virtual ICollection<Objekat> Objekats { get; set; }

    }
}
