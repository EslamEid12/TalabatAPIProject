using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using TalabatAPIProject.Error;

namespace TalabatAPIProject.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentServices _paymentServices;
        private readonly ILogger<PaymentsController> _logger;
        private const string WhSecret = "whsec_729108f85820f70b9e111eb5845bed3e74f155a1f08beef5d3aa26f449d0b549";


        public PaymentsController(IPaymentServices paymentServices,ILogger<PaymentsController> logger)
        {
            _paymentServices = paymentServices;
            _logger = logger;
        }
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>>CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentServices.CreateOrUpadtePaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiResponse(404, "An Eror With Your Basket"));
            return Ok(basket);
        }
        [HttpPost("")]
        public async Task<IActionResult> StripWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], WhSecret);
                var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;
                Order order;

                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        order = await _paymentServices.UpdatePaymentIntetToSucceededOrFialed(paymentIntent.Id, true);
                        _logger.LogInformation("Payment Is Succeeded ya hamada ", paymentIntent.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order = await _paymentServices.UpdatePaymentIntetToSucceededOrFialed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment Is Failed ya hamada :( ", paymentIntent.Id);

                        break;
                }


                // Handle the event
               ///if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
               ///{
               ///   order= await _paymentServices.UpdatePaymentIntetToSucceededOrFialed(paymentIntent.Id,false);
               ///
               ///}
               ///else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
               ///{
               ///}
               /// ... handle other event types
               ///else
               ///{
               ///    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
               ///}

                return Ok();
           
        }
    }

    }

