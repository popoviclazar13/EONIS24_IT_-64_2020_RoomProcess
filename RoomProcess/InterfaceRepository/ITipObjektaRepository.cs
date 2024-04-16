using RoomProcess.Models.Entities;

namespace RoomProcess.InterfaceRepository
{
    public interface ITipObjektaRepository
    {
        ICollection<TipObjekta> GetTipObjektas();
        TipObjekta GetTipObjektaById(int tipObjektaId);
        bool TipObjektaExist(int tipObjektaId);
        bool CreateTipObjekta(TipObjekta tipObjekta);
        bool UpdateTipObjekta(TipObjekta tipObjekta);
        bool DeleteTipObjekta(TipObjekta tipObjekta);
        bool Save();
    }
}
