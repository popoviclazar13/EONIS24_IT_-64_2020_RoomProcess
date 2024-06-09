namespace RoomProcess.Models.DTO
{
    public class RacunDTO
    {
        public int Id { get; set; }
        public int RezervacijaID { get; set; }
        public string RacunID { get; set; }
        public DateTime Datum { get; set; }
        public int UkupnaCena { get; set; }
        public string Status { get; set; }
    }
}
