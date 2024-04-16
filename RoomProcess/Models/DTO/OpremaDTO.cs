using System.ComponentModel.DataAnnotations.Schema;

namespace RoomProcess.Models.DTO
{
    public class OpremaDTO
    {
        public int OpremaId { get; set; }
        public string OpremaNaziv { get; set; }
        public int ObjekatId { get; set; }
        public ObjekatDTO ObjekatDTO { get; set; }
    }
}
