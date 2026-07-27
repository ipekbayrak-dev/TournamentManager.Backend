using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Payment;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController(IPaymentService _paymentService, ILogger<PaymentController> _logger) : ControllerBase
    {
        [HttpGet("entry/{tournamentEntryId}")]
        public async Task<IActionResult> GetAllAsync(Guid tournamentEntryId)
        {
            try
            {
                var result = await _paymentService.GetAllByTournamentEntryIdAsync(tournamentEntryId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.Data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during GetAll for {TournamentEntryId}", tournamentEntryId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _paymentService.GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                if (result.Data is null)
                {
                    return NotFound();
                }

                return Ok(result.Data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during GetById for {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePaymentRequest createPaymentRequest)
        {
            try
            {
                var result = await _paymentService.CreateAsync(createPaymentRequest);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.Data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Create for {StripeSessionId}", createPaymentRequest.StripeSessionId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
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

                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Update for {Id}", updatePaymentRequest.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _paymentService.DeleteAsync(id);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Delete for {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}