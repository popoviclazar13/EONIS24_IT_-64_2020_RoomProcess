using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;

namespace RoomProcess.Repository
{
    public class PopustRepository : IPopustRepository
    {

        private readonly DataContext _dataContext;
        public PopustRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool CreatePopust(Popust popust)
        {
            _dataContext.Add(popust);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeletePopust(Popust popust)
        {
            _dataContext.Remove(popust);
            return Save();
        }

        public Popust GetPopustById(int popustId)
        {
            return _dataContext.Popust.Where(k => k.PopustId == popustId).FirstOrDefault();
        }

        public ICollection<Popust> GetPopusts()
        {
            return _dataContext.Popust.OrderBy(k => k.PopustId).ToList();
        }

        public bool PopustExist(int popustId)
        {
            return _dataContext.Popust.Any(k => k.PopustId == popustId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePopust(Popust popust)
        {
            _dataContext.Update(popust);
            return Save();
        }
    }
}
