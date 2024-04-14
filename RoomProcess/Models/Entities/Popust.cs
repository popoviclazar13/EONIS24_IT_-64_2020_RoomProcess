using System.ComponentModel.DataAnnotations;

namespace RoomProcess.Models.Entities
{
    public class Popust
    {
        [Key]
        public int PopustId { get; set; }
        public string PopustNaziv { get; set; }
        public int PopustIznos { get; set; }

        public Popust()
        {

        }
    }
}
