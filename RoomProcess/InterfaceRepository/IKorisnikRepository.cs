using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface IKorisnikRepository
    {
        ICollection<Korisnik> GetKorisniks();
        Korisnik GetKorisnikById(int id);
        bool KorisnikExist(int KorisnikId);
        bool CreateKorisnik(Korisnik korisnik);
        bool UpdateKorisnik(Korisnik korisnik);
        bool DeleteKorisnik(Korisnik korisnik);
        bool Save();
        ICollection<Korisnik> GetKorisnikByIdUloga(int ulogaId);
    }
}
