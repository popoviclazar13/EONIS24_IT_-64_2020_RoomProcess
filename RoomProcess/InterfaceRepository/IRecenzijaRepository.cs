using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface IRecenzijaRepository
    {
        ICollection<Recenzija> GetRecenzijas();
        Recenzija GetRecenzijaById(int recenzijaId);
        bool RecenzijaExist(int recenzijaId);
        bool CreateRecenzija(Recenzija recenzija);
        bool UpdateRecenzija(Recenzija recenzija);
        bool DeleteRecenzija(Recenzija recenzija);
        bool Save();
    }
}
