using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RoomProcess.Helpers;
using Microsoft.Extensions.Logging; // Dodajte ovu using direktivu
using RoomProcess.Models.Entities;
using RoomProcess.Services.RacunService;
using Stripe;
using Stripe.Checkout;

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

        public RacunController(IOptions<StripeSettings> stripeSettings, ILogger<RacunController> logger)
        {
            _stripeSettings = stripeSettings.Value;
            _logger = logger;
        }

        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession(decimal cena)
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
                    // Process the checkout session as needed
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Error while processing Stripe webhook");
                return BadRequest();
            }
        }
    }
}
