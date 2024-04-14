using System.ComponentModel.DataAnnotations;

namespace RoomProcess.Models.Entities
{
    public class TipObjekta
    {
        [Key]
        public int TipObjektaId { get; set; }
        public string TipObjektaNaziv { get; set; }

        public TipObjekta() { }
    }
}
