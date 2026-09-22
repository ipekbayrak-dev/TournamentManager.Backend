using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Payment;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController(
        IPaymentService _paymentService,
        IConfiguration _configuration,
        ILogger<PaymentController> _logger) : ControllerBase
    {
        [HttpGet("entry/{tournamentEntryId}")]
        public async Task<IActionResult> GetAllAsync(Guid tournamentEntryId)
        {
            try
            {
                var result = await _paymentService.GetAllByTournamentEntryIdAsync(tournamentEntryId);
                if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during GetAll for {TournamentEntryId}", tournamentEntryId);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _paymentService.GetByIdAsync(id);
                if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
                if (result.Data is null) return NotFound();
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during GetById for {Id}", id);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSessionAsync([FromBody] CreateCheckoutSessionRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
                var frontendBaseUrl = _configuration["FrontendSettings:BaseUrl"]!.TrimEnd('/');

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "usd",
                                UnitAmount = 1000,
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"Tournament Entry — {request.TournamentName}",
                                    Description = "Entry fee for tournament registration"
                                }
                            },
                            Quantity = 1,
                        }
                    },
                    Mode = "payment",
                    SuccessUrl = $"{frontendBaseUrl}/Payment/Success?slug={request.TournamentSlug}&session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{frontendBaseUrl}/Tournament/Detail/{request.TournamentSlug}",
                    Metadata = new Dictionary<string, string>
                    {
                        { "tournamentEntryId", request.TournamentEntryId.ToString() }
                    }
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                await _paymentService.CreateAsync(new CreatePaymentRequest
                {
                    TournamentEntryId = request.TournamentEntryId,
                    Amount = 10.00m,
                    Currency = "USD",
                    StripeSessionId = session.Id
                });

                return Ok(new CreateCheckoutSessionResponse { SessionUrl = session.Url });
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error during checkout session creation");
                return BadRequest("Payment service error. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during checkout session creation");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> WebhookAsync()
        {
            string json;
            using (var reader = new StreamReader(HttpContext.Request.Body))
                json = await reader.ReadToEndAsync();

            var webhookSecret = _configuration["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret
                );

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = (Session)stripeEvent.Data.Object;

                    if (session.Metadata.TryGetValue("tournamentEntryId", out var entryIdStr)
                        && Guid.TryParse(entryIdStr, out var entryId))
                    {
                        var paymentsResult = await _paymentService.GetAllByTournamentEntryIdAsync(entryId);
                        var payment = paymentsResult.Data?.FirstOrDefault(p => p.StripeSessionId == session.Id);

                        if (payment is not null)
                        {
                            await _paymentService.UpdateAsync(new UpdatePaymentRequest
                            {
                                Id = payment.Id,
                                Status = PaymentStatus.Succeeded,
                                StripePaymentIntentId = session.PaymentIntentId,
                                PaidAt = DateTime.UtcNow
                            });
                        }
                    }
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe webhook verification failed");
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePaymentRequest createPaymentRequest)
        {
            try
            {
                var result = await _paymentService.CreateAsync(createPaymentRequest);
                if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Create for {StripeSessionId}", createPaymentRequest.StripeSessionId);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdatePaymentRequest updatePaymentRequest)
        {
            try
            {
                updatePaymentRequest.Id = id;
                var result = await _paymentService.UpdateAsync(updatePaymentRequest);
                if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Update for {Id}", updatePaymentRequest.Id);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _paymentService.DeleteAsync(id);
                if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Delete for {Id}", id);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
