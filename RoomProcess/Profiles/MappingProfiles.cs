using AutoMapper;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;

namespace RoomProcess.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            //Korisnik
            CreateMap<Korisnik, KorisnikDTO>();
            CreateMap<KorisnikDTO, Korisnik>();

        }
    }
}
