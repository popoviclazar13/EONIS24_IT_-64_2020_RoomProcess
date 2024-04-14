using Microsoft.IdentityModel.Tokens;
using RoomProcess.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoomProcess.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IConfiguration _config;

        public AuthHelper(IConfiguration config)
        {
            _config = config;
        }

        public void CreatePasswordHash(string? password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            throw new NotImplementedException();
        }

        public string CreateToken(Korisnik korisnik)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Username", korisnik.Email),
                new Claim("UserId", korisnik.KorisnikId.ToString()),
                new Claim("Role", GetRole(korisnik))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value)); //key from appsettings.json

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string GetClaim(string token, string claimType)
        {
            throw new NotImplementedException();
        }

        public bool ValidateCurrentToken(string token)
        {
            throw new NotImplementedException();
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            throw new NotImplementedException();
        }

        private string GetRole(Korisnik korisnik)
        {

            if (korisnik.UlogaId == 1)
                return "Admin";
            else if (korisnik.UlogaId == 2)
                return "Vlasnik";
            else
                return "Gost";

        }

    }
}
