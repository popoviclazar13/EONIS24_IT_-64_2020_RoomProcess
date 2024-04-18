using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface IOpremaRepository
    {
        ICollection<Oprema> GetOpremas();
        Oprema GetOpremaById(int opremaId);
        bool OpremaExist(int opremaId);
        bool CreateOprema(Oprema oprema);
        bool UpdateOprema(Oprema oprema);
        bool DeleteOprema(Oprema oprema);
        bool Save();
        ICollection<Oprema> GetOpremaByIdObjekat(int objekatId);
    }
}
