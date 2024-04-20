using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface IObjekatRepository
    {
        ICollection<Objekat> GetObjekats();
        Objekat GetObjekatById(int objekatId);
        bool ObjekatExist(int objekatId);
        bool CreateObjekat(Objekat objekat);
        bool UpdateObjekat(Objekat objekat);
        bool DeleteObjekat(Objekat objekat);
        bool Save();
        Objekat GetObjekatByNaziv(string naziv);
        ICollection<Objekat> GetObjekatByPriceRange(int cenaDonja, int cenaGornja);
        ICollection<Objekat> GetObjekatByGrad(string grad);
        ICollection<Objekat> GetObjekatByIdKorisnik(int korisnikId);
        ICollection<Objekat> GetObjekatByIdTipObjekta(int tipObjektaId);
        ICollection<Objekat> GetObjekatByIdPopust(int popustId);
    }
}
