using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RoomProcess.Helpers;
using Microsoft.Extensions.Logging; // Dodajte ovu using direktivu
using RoomProcess.Models.Entities;
using RoomProcess.Services.RacunService;
using Stripe;
using Stripe.Checkout;
using RoomProcess.Repository;
using RoomProcess.InterfaceRepository;

namespace RoomProcess.Controllers
{
    public class RacunController : ControllerBase
    {
        /*private readonly IRacunService racunService;
        private readonly IConfiguration config;
        public RacunController(IRacunService racunService, IConfiguration config)
        {
            this.racunService = racunService;
            this.config = config;
        }

        [HttpPost("plati")]
        public Task<dynamic> Racun(RacunData plati)
        {
            return racunService.Racun(plati);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], config["StripeSettings:WhSecret"]);

                if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    var charge = stripeEvent.Data.Object as Charge;
                }
                else
                {
                    return BadRequest($"Unexpected event type: {stripeEvent.Type}");
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest($"StripeException: {e.Message}");
            }
            catch (Exception e)
            {
                return BadRequest($"Exception: {e.Message}");
            }


        }*/
        private readonly StripeSettings _stripeSettings;
        private readonly ILogger<RacunController> _logger; // Dodajte ovde ILogger
        private readonly IRezervacijaRepository _rezervacijaRepository;

        public RacunController(IOptions<StripeSettings> stripeSettings, ILogger<RacunController> logger, IRezervacijaRepository rezervacijaRepository)
        {
            _stripeSettings = stripeSettings.Value;
            _logger = logger;
            _rezervacijaRepository = rezervacijaRepository;
        }

        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession(decimal cena, int rezervacijaId)
        {
            _logger.LogInformation("Creating checkout session for amount: {Cena}", cena);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(cena / 105 * 100), // Cena u centima
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Rezervacija",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/dashbord", // URL za uspešno plaćanje
                CancelUrl = "http://localhost:4200/dashbord", // URL za neuspešno plaćanje
            };

            var service = new SessionService();
            Session session = service.Create(options);

            try
            {
                // Poziv servisa za rezervaciju
                 // Prilagodite naziv servisa prema vašem projektu
                var rezervacija = _rezervacijaRepository.GetRezervacijaById(rezervacijaId); // Implementirajte logiku za pronalaženje rezervacije

                if (rezervacija != null)
                {
                    // Ažuriranje potvrde rezervacije
                    rezervacija.Potvrda = true; // Postavite vrednost potvrdeRezervacije na true
                    _rezervacijaRepository.Save(); // Implementirajte logiku za ažuriranje rezervacije
                }
                else
                {
                    _logger.LogError("Rezervacija sa ID {RezervacijaId} nije pronađena.", rezervacijaId);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom ažuriranja rezervacije sa ID {RezervacijaId}.", rezervacijaId);
                return StatusCode(500, "Greška prilikom obrade rezervacije.");
            }

            //return Ok(new { sessionId = session.Id });
            return Ok(new { sessionId = session.Url });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            _logger.LogInformation("Processing Stripe webhook...");

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeSettings.WhSecret
                );

                // Handle the event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    
                    if (session != null)
                    {
                        var paymentIntentService = new PaymentIntentService();
                        var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                        var paymentDate = paymentIntent.Created;
                        var paymentSuccessful = paymentIntent.Status;

                        return Ok(new
                        {
                            message = "Plaćanje je izvršeno.",
                            paymentDate = paymentDate,
                            status = paymentSuccessful
                        });
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Error while processing Stripe webhook");
                return BadRequest();
            }
        }
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var options = new BalanceTransactionListOptions
                {
                    Limit = 20, // Limit the number of transactions to retrieve
                };

                var service = new BalanceTransactionService();
                var transactions = await service.ListAsync(options);

                var simplifiedTransactions = new List<object>();
                foreach (var transaction in transactions)
                {
                    var simplifiedTransaction = new
                    {
                        Amount = transaction.Amount,
                        Customer = transaction.Id,
                        Currency = transaction.Currency,
                        Date = transaction.Created,
                        Status = transaction.Status,
                    };
                    simplifiedTransactions.Add(simplifiedTransaction);
                }
                return Ok(simplifiedTransactions);
            }
            catch (StripeException e)
            {
                return StatusCode((int)e.HttpStatusCode, new { error = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching transactions." });
            }
        }
    }
}
