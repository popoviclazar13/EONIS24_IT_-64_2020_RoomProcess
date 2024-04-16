using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;

namespace RoomProcess.Repository
{
    public class OpremaRepository : IOpremaRepository
    {
        private readonly DataContext _dataContext;
        public OpremaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateOprema(Oprema oprema)
        {
            _dataContext.Add(oprema);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeleteOprema(Oprema oprema)
        {
            _dataContext.Remove(oprema);
            return Save();
        }

        public Oprema GetOpremaById(int opremaId)
        {
            return _dataContext.Oprema.Where(k => k.OpremaId == opremaId)
                            .Include(x => x.Objekat)
                                   .FirstOrDefault();
        }

        public ICollection<Oprema> GetOpremas()
        {
            return _dataContext.Oprema.OrderBy(k => k.OpremaId)
                .Include(x => x.Objekat)
                                        .ToList();
        }

        public bool OpremaExist(int opremaId)
        {
            return _dataContext.Oprema.Any(k => k.OpremaId == opremaId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOprema(Oprema oprema)
        {
            _dataContext.Update(oprema);
            return Save();
        }
    }
}
