using RoomProcess.Models.Entities;

namespace RoomProcess.Services.RacunService
{
    public interface IRacunService
    {
        Task<dynamic> Racun(RacunData plati);
    }
}
