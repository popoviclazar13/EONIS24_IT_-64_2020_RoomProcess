using System.ComponentModel.DataAnnotations;

namespace RoomProcess.Models.Entities
{
    public class Uloga
    {
        [Key]
        public int UlogaId { get; set; }
        public string UlogaNaziv { get; set; }

        public Uloga() { }
    }
}
