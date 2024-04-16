using RoomProcess.Data;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.Entities;
using System.Runtime.Intrinsics.X86;

namespace RoomProcess.Repository
{
    public class TipObjektaRepository : ITipObjektaRepository
    {
        private readonly DataContext _dataContext;
        public TipObjektaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool CreateTipObjekta(TipObjekta tipObjekta)
        {
            _dataContext.Add(tipObjekta);
            _dataContext.SaveChanges();
            return Save();
        }

        public bool DeleteTipObjekta(TipObjekta tipObjekta)
        {
            _dataContext.Remove(tipObjekta);
            return Save();
        }

        public TipObjekta GetTipObjektaById(int tipObjektaId)
        {
            return _dataContext.TipObjekta.Where(k => k.TipObjektaId == tipObjektaId).FirstOrDefault();
        }

        public ICollection<TipObjekta> GetTipObjektas()
        {
            return _dataContext.TipObjekta.OrderBy(k => k.TipObjektaId).ToList();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TipObjektaExist(int tipObjektaId)
        {
            return _dataContext.TipObjekta.Any(k => k.TipObjektaId == tipObjektaId);
        }

        public bool UpdateTipObjekta(TipObjekta tipObjekta)
        {
            _dataContext.Update(tipObjekta);
            return Save();
        }
    }
}
