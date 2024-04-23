using Microsoft.IdentityModel.Tokens;
using RoomProcess.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public string CreateToken(Korisnik korisnik)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, korisnik.Email),    //JwtRegisteredClaimNames.Email
                new Claim(ClaimTypes.Name, korisnik.Ime),
                new Claim(ClaimTypes.Role, GetRole(korisnik)),
                new Claim("UserId", korisnik.KorisnikId.ToString()),
                //new Claim("Role", GetRole(korisnik))
                //new Claim(ClaimTypes.Role, GetRole(korisnik))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value)); //key from appsettings.json

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(10),//traje 10 dana
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

            /*var tokenDeskriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), //kolko dana traje
                SigningCredentials = creds,
                Issuer = _config["AppSettings:Issuer"]
            };*/

            /*var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDeskriptor);

            return tokenHandler.WriteToken(token);*/
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var clamVariable = securityToken.Claims;
            //
            if (securityToken.Claims.Count() == 0)
            {
                // Lista claim-ova je prazna
                Console.WriteLine("Lista claim-ova je prazna.");
            }
            else
            {
                // Lista claim-ova nije prazna
                Console.WriteLine($"Lista claim-ova sadrži {securityToken.Claims.Count()} claim-ova.");
            }
            //
            //var claimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            var claim = securityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role); //MORA VAKO
            if (claim != null)
            {
                var claimValue = claim.Value;
                return claimValue;
                // Nastavite sa radom sa claimValue
            }
            else
            {
                return claimType;
            }
            //return claimValue;
        }

        public bool ValidateCurrentToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value)); //key from appsettings.json

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false //MOrali izmeniti
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash); //comparing saved and computed hash
            }
        }

        private string GetRole(Korisnik korisnik)
        {

            if (korisnik.UlogaId == 1)
                return "Admin";
            else if (korisnik.UlogaId == 2)
                return "Vlasnik";
            else
                return "Korisnik";

        }


    }
}
