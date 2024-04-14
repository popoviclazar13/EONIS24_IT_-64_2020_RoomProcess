using RoomProcess.Models.Entities;

namespace RoomProcess.Helpers
{
    public interface IAuthHelper
    {
        public string CreateToken(Korisnik korisnik);
        public bool ValidateCurrentToken(string token);
        public void CreatePasswordHash(string? password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        public string GetClaim(string token, string claimType);
    }
}
