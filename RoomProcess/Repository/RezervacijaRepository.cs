using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;

namespace RoomProcess.Repository
{
    public class RezervacijaRepository : IRezervacijaRepository
    {
        private readonly DataContext _dataContext;
        public RezervacijaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool CreateRezervacija(Rezervacija rezervacija)
        {
            _dataContext.Add(rezervacija);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeleteRezervacija(Rezervacija rezervacija)
        {
            _dataContext.Remove(rezervacija);
            return Save();
        }

        public Rezervacija GetRezervacijaById(int rezervacijaId)
        {
            return _dataContext.Rezervacija.Where(k => k.RezervacijaId == rezervacijaId)
                            .Include(x => x.Korisnik)
                            .Include(x => x.Objekat)
                                   .FirstOrDefault();
        }

        public ICollection<Rezervacija> GetRezervacijas()
        {
            return _dataContext.Rezervacija.OrderBy(k => k.RezervacijaId)
                .Include(x => x.Korisnik)
                .Include(x => x.Objekat)
                                        .ToList();
        }

        public bool RezervacijaExist(int rezervacijaId)
        {
            return _dataContext.Rezervacija.Any(k => k.RezervacijaId == rezervacijaId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRezervacija(Rezervacija rezervacija)
        {
            _dataContext.Update(rezervacija);
            return Save();
        }
    }
}
