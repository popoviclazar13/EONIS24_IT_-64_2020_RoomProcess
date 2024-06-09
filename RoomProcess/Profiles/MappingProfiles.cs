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

            //Racun
            CreateMap<Racun, RacunDTO>();
            CreateMap<RacunDTO, Racun>();

            //Recenzija
            CreateMap<Recenzija, RecenzijaDTO>();
            CreateMap<RecenzijaDTO, Recenzija>();

            CreateMap<RecenzijaUpdateDTO, Recenzija>();
            CreateMap<Recenzija, RecenzijaUpdateDTO>();

            CreateMap<RecenzijaCreateDTO, Recenzija>();
            CreateMap<Recenzija, RecenzijaCreateDTO>();

            //Rezervacija
            CreateMap<Rezervacija, RezervacijaDTO>();
            CreateMap<RezervacijaDTO, Rezervacija>();

            CreateMap<RezervacijaUpdateDTO, Rezervacija>();
            CreateMap<Rezervacija, RezervacijaUpdateDTO>();

            CreateMap<RezervacijaCreateDTO, Rezervacija>();
            CreateMap<Rezervacija, RezervacijaCreateDTO>();

            //Objekat
            CreateMap<Objekat, ObjekatDTO>();
            CreateMap<ObjekatDTO, Objekat>();

            CreateMap<Objekat, ObjekatCreateDTO>();
            CreateMap<ObjekatCreateDTO, Objekat>();

            CreateMap<Objekat, ObjekatUpdateDTO>();
            CreateMap<ObjekatUpdateDTO, Objekat>();

            //Uloga
            CreateMap<Uloga, UlogaDTO>();
            CreateMap<UlogaDTO, Uloga>();

            //TipObjekta
            CreateMap<TipObjekta, TipObjektaDTO>();
            CreateMap<TipObjektaDTO, TipObjekta>();

            //Oprema
            CreateMap<Oprema, OpremaDTO>();
            CreateMap<OpremaDTO, Oprema>();

            //Popust
            CreateMap<Popust, PopustDTO>();
            CreateMap<PopustDTO, Popust>();

        }
    }
}
