using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface IUlogaRepository
    {
        ICollection<Uloga> GetUlogas();
        Uloga GetUlogaById(int ulogaId);
        bool UlogaExist(int ulogaId);
        bool CreateUloga(Uloga uloga);
        bool UpdateUloga(Uloga uloga);
        bool DeleteUloga(Uloga uloga);
        bool Save();
    }
}
