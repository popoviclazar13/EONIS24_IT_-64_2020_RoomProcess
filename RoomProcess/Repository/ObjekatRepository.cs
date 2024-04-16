using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;

namespace RoomProcess.Repository
{
    public class ObjekatRepository : IObjekatRepository
    {
        private readonly DataContext _dataContext;
        public ObjekatRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool CreateObjekat(Objekat objekat)
        {
            _dataContext.Add(objekat);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeleteObjekat(Objekat objekat)
        {
            _dataContext.Remove(objekat);
            return Save();
        }

        public Objekat GetObjekatById(int objekatId)
        {
            return _dataContext.Objekat.Where(k => k.ObjekatId == objekatId)
                            .Include(x => x.Korisnik)
                            .Include(x => x.TipObjekta)
                            .Include(x => x.Popust)
                                   .FirstOrDefault();
        }

        public ICollection<Objekat> GetObjekats()
        {
            return _dataContext.Objekat.OrderBy(k => k.ObjekatId)
                .Include(x => x.Korisnik)
                .Include(x => x.TipObjekta)
                .Include(x => x.Popust)
                                        .ToList();
        }

        public bool ObjekatExist(int objekatId)
        {
            return _dataContext.Objekat.Any(k => k.ObjekatId == objekatId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateObjekat(Objekat objekat)
        {
            _dataContext.Update(objekat);
            return Save();
        }
    }
}
