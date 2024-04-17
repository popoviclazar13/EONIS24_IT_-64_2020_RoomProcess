using System.ComponentModel.DataAnnotations;

namespace RoomProcess.Models.DTO
{
    public class KorisnikRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.",
         MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int UlogaId { get; set; }

    }
}
