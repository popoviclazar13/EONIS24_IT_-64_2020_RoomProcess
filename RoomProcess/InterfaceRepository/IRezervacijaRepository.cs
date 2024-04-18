using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface IRezervacijaRepository
    {
        ICollection<Rezervacija> GetRezervacijas();
        Rezervacija GetRezervacijaById(int rezervacijaId);
        bool RezervacijaExist(int rezervacijaId);
        bool CreateRezervacija(Rezervacija rezervacija);
        bool UpdateRezervacija(Rezervacija rezervacija);
        bool DeleteRezervacija(Rezervacija rezervacija);
        bool Save();
        ICollection<Rezervacija> GetRezervacijaByIdObjekat(int objekatId);
        ICollection<Rezervacija> GetRezervacijaByIdKorisnik(int korisnikId);
    }
}
