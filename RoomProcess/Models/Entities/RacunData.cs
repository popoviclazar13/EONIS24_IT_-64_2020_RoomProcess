namespace RoomProcess.Models.Entities
{
    public class RacunData
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Cvc { get; set; }
        public int RezervacijaID { get; set; }
        public int UkupnaCena { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Kontakt { get; set; }

    }
}
