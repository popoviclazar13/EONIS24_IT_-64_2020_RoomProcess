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
    }
}
