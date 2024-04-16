using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface IPopustRepository
    {
        ICollection<Popust> GetPopusts();
        Popust GetPopustById(int popustId);
        bool PopustExist(int popustId);
        bool CreatePopust(Popust popust);
        bool UpdatePopust(Popust popust);
        bool DeletePopust(Popust popust);
        bool Save();
    }
}
