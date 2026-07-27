using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.PrizeAllocation;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrizeAllocationController(IPrizeAllocationService _prizeAllocationService, ILogger<PrizeAllocationController> _logger) : ControllerBase
    {
        [HttpGet("prize/{prizeId}")]
        public async Task<IActionResult> GetAllAsync(Guid prizeId)
        {
            try
            {
                var result = await _prizeAllocationService.GetAllByPrizeIdAsync(prizeId);

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
                _logger.LogError(ex, "Unexpected error during GetAll for {PrizeId}", prizeId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _prizeAllocationService.GetByIdAsync(id);

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
        public async Task<IActionResult> CreateAsync([FromBody] CreatePrizeAllocationRequest createPrizeAllocationRequest)
        {
            try
            {
                var result = await _prizeAllocationService.CreateAsync(createPrizeAllocationRequest);

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
                _logger.LogError(ex, "Unexpected error during Create for {PrizeId}", createPrizeAllocationRequest.PrizeId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdatePrizeAllocationRequest updatePrizeAllocationRequest)
        {
            try
            {
                updatePrizeAllocationRequest.Id = id;
                var result = await _prizeAllocationService.UpdateAsync(updatePrizeAllocationRequest);

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
                _logger.LogError(ex, "Unexpected error during Update for {Id}", updatePrizeAllocationRequest.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _prizeAllocationService.DeleteAsync(id);

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