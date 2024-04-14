using RoomProcess.Models;
using RoomProcess.Models.DTO;

namespace RoomProcess.Services.KorisnikService
{
    public interface IKorisnikService
    {
        Response<KorisnikDTO> GetKorisnikByID(int id);
        Response<string> LoginKorisnik(KorisnikLoginDTO korisnikLoginDTO); // Za logovanje
        Response<string> CreateKorisnik(KorisnikRequestDTO korisnikRequestDTO); // Za Kreiranje
        ResponseNoData UpdateKorisnik(int id, KorisnikRequestDTO korisnikRequestDTO);
        ResponseNoData DeleteKorisnik(int id);
    }
}
