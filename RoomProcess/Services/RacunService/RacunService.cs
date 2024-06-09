using AutoMapper;
using RoomProcess.Data;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;
using Stripe;

namespace RoomProcess.Services.RacunService
{
    public class RacunService : IRacunService
    {
        private readonly IMapper mapper;
        private readonly DataContext _context;
        public RacunService(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            _context = context;
        }
        //metoda mora biti asynhrona!!
        public async Task<dynamic> Racun(RacunData plati)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51PMpAtP9NqKe2sYzlGKOlZRgSoPxs0GXgOctIlhh5QNqGQTWpqaaq4h5I84Ka7N6u0okhddTnYSbQCCZj0kg9pFq00FkbsXpRq";

                var optionsToken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = plati.CardNumber,
                        ExpMonth = plati.Month,
                        ExpYear = plati.Year,
                        Cvc = plati.Cvc
                    }
                };

                //moramo staviti da je asynhrona posto cekamo odgvoor
                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(optionsToken);



                var options = new ChargeCreateOptions
                {
                    Amount = plati.UkupnaCena * 100,
                    Currency = "eur",
                    Source = stripeToken.Id,
                    ReceiptEmail = plati.Email
                };

                var service = new ChargeService();
                Charge charge = await service.CreateAsync(options);

                if (charge.Paid)
                {
                    var racunDTO = new RacunDTO
                    {
                        RezervacijaID = plati.RezervacijaID,
                        RacunID = charge.Id,
                        Datum = charge.Created,
                        UkupnaCena = (int)(charge.Amount / 100),
                        Status = charge.Status,
                    };

                    var paymentEntry = mapper.Map<Racun>(racunDTO);
                    _context.Racun.Add(paymentEntry);
                    _context.SaveChanges();
                    return racunDTO.Status;
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
