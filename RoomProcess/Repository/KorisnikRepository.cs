
using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;

namespace RoomProcess.Repository
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly DataContext _dataContext;
        public KorisnikRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateKorisnik(Korisnik korisnik)
        {
            _dataContext.Add(korisnik);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeleteKorisnik(Korisnik korisnik)
        {
            _dataContext.Remove(korisnik);
            return Save();
        }

        public Korisnik GetKorisnikById(int id)
        {
            return _dataContext.Korisnik.Where(k => k.KorisnikId == id)
                            .Include(x => x.Uloga)
                                   .FirstOrDefault();
        }

        public ICollection<Korisnik> GetKorisniks()
        {
            return _dataContext.Korisnik.OrderBy(k => k.KorisnikId)
                .Include(x => x.Uloga)
                                        .ToList();
        }

        public bool KorisnikExist(int KorisnikId)
        {
            return _dataContext.Korisnik.Any(k => k.KorisnikId == KorisnikId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateKorisnik(Korisnik korisnik)
        {
            _dataContext.Update(korisnik);
            return Save();
        }
    }
}
