using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.Helpers;
using RoomProcess.Models;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;

namespace RoomProcess.Services.KorisnikService
{
    public class KorisnikService : IKorisnikService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IAuthHelper auth;

        public KorisnikService(IMapper mapper, DataContext context, IAuthHelper auth)
        {
            _mapper = mapper;
            _dbContext = context;
            this.auth = auth;
        }

        public Response<string> CreateKorisnik(KorisnikRequestDTO korisnikRequestDTO)
        {
            var ret = new Response<string>();

            var user = _dbContext.Korisnik.FirstOrDefault(x => x.Ime == korisnikRequestDTO.Ime);

            if (user != null)
            {
                ret.Message = $"User already exists";
                ret.Status = 409;
            }
            else
            {
                auth.CreatePasswordHash(korisnikRequestDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                Korisnik newUser = new Korisnik();
                newUser.Password = korisnikRequestDTO.Password;
                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;
                newUser.Ime = korisnikRequestDTO.Ime;
                newUser.Prezime = korisnikRequestDTO.Prezime;
                newUser.Email = korisnikRequestDTO.Email;

                //newUser.UlogaId = _dbContext.Uloga.FirstOrDefault(x => x.UlogaNaziv == "Korinsik").UlogaId;
                newUser.UlogaId = korisnikRequestDTO.UlogaId;
                _dbContext.Korisnik.Add(newUser);
                _dbContext.SaveChanges();

                ret.Message = $"Successfully registered User {korisnikRequestDTO.Ime}";
                string token = auth.CreateToken(newUser);
                ret.TransferObject = token;


            }

            return ret;
        }

        public ResponseNoData DeleteKorisnik(int korisnikId)
        {
            var ret = new ResponseNoData();

            var user = _dbContext.Korisnik.FirstOrDefault(x => x.KorisnikId == korisnikId);

            if (user == null)
            {
                ret.Status = 404;
                ret.Message = $"There is no User with ID {korisnikId}";
            }
            else
            {
                _dbContext.Korisnik.Remove(user);
                _dbContext.SaveChanges();
                ret.Status = 201;
                ret.Message = $"Successfully deleted User '{user.Ime}' with ID {korisnikId}";
            }
            return ret;
        }

        public Response<KorisnikDTO> GetKorisnikByID(int korisnikId)
        {
            var ret = new Response<KorisnikDTO>();

            var user = _dbContext.Korisnik.FirstOrDefault(x => x.KorisnikId == korisnikId);

            if (user == null)
            {
                ret.Status = 404;
                ret.Message = $"There is no User with ID {korisnikId}";
            }
            else
            {
                ret.TransferObject = _mapper.Map<KorisnikDTO>(user);

            }

            return ret;
        }

        public Response<string> LoginKorisnik(KorisnikLoginDTO korisnikLoginDTO)
        {
            var ret = new Response<string>();

            var userFromDb = _dbContext.Korisnik.FirstOrDefault(u => u.Email == korisnikLoginDTO.Email)
                ?? _dbContext.Korisnik.OfType<Korisnik>().FirstOrDefault(u => u.Email == korisnikLoginDTO.Email);


            if (userFromDb != null)
            {
                if (auth.VerifyPasswordHash(korisnikLoginDTO.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
                {
                    string token = auth.CreateToken(userFromDb);

                    ret.Message = $"You logged in successfully";
                    ret.TransferObject = token;
                    //Dodato zbog Logovanja da znamo koja je uloga
                    ret.Role = userFromDb.UlogaId;
                    //Dodato zbog Logovanja da znamo koji korisnik je ulogovan
                    ret.Name = userFromDb.Ime;
                }
                else
                {
                    ret.Message = $"Password is not correct";
                    ret.Status = 401;
                }
            }
            else
            {
                ret.Message = $"Username {korisnikLoginDTO.Email} doesn't exist";
                ret.Status = 404;
            }

            return ret;
        }

        public ResponseNoData UpdateKorisnik(int korisnikId, KorisnikRequestDTO korisnikRequestDTO)
        {
            var ret = new ResponseNoData();

            var user = _dbContext.Korisnik.FirstOrDefault(x => x.KorisnikId == korisnikId);

            if (user == null)
            {
                ret.Status = 404;
                ret.Message = $"There is no User with ID {korisnikId}";
                return ret;
            }

            try
            {
                _dbContext.Entry(user).CurrentValues.SetValues(korisnikRequestDTO);
                _dbContext.SaveChanges();
                ret.Message = $"User '{user.Ime}' successfully updated";
            }
            catch
            {
                ret.Message = "Something went wrong";
                ret.Status = 400;
            }

            return ret;
        }
    }
}
