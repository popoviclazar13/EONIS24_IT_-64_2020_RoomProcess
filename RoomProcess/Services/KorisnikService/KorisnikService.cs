using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoomProcess.Helpers;
using RoomProcess.Models;
using RoomProcess.Models.DTO;

namespace RoomProcess.Services.KorisnikService
{
    public class KorisnikService : IKorisnikService
    {

        private readonly Mapper _mapper;
        private readonly DbContext _dbContext;
        private readonly IAuthHelper auth;


        public Response<string> CreateKorisnik(KorisnikRequestDTO korisnikRequestDTO)
        {
            throw new NotImplementedException();
        }

        public ResponseNoData DeleteKorisnik(int id)
        {
            throw new NotImplementedException();
        }

        public Response<KorisnikDTO> GetKorisnikByID(int id)
        {
            throw new NotImplementedException();
        }

        public Response<string> LoginKorisnik(KorisnikLoginDTO korisnikLoginDTO)
        {
            throw new NotImplementedException();
        }

        public ResponseNoData UpdateKorisnik(int id, KorisnikRequestDTO korisnikRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
