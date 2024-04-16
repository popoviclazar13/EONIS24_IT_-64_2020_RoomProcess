using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;

namespace RoomProcess.Repository
{
    public class UlogaRepository : IUlogaRepository
    {
        private readonly DataContext _dataContext;
        public UlogaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateUloga(Uloga uloga)
        {
            _dataContext.Add(uloga);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeleteUloga(Uloga uloga)
        {
            _dataContext.Remove(uloga);
            return Save();
        }

        public Uloga GetUlogaById(int ulogaId)
        {
            return _dataContext.Uloga.Where(k => k.UlogaId == ulogaId).FirstOrDefault();
        }

        public ICollection<Uloga> GetUlogas()
        {
            return _dataContext.Uloga.OrderBy(k => k.UlogaId).ToList();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UlogaExist(int ulogaId)
        {
            return _dataContext.Uloga.Any(k => k.UlogaId == ulogaId);
        }

        public bool UpdateUloga(Uloga uloga)
        {
            _dataContext.Update(uloga);
            return Save();
        }
    }
}
