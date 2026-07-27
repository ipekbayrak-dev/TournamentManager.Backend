using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.TournamentEntry;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TournamentEntryController(ITournamentEntryService _tournamentEntryService, ILogger<TournamentEntryController> _logger) : ControllerBase
    {
        [HttpGet("tournament/{tournamentId}")]
        public async Task<IActionResult> GetAllAsync(Guid tournamentId)
        {
            try
            {
                var result = await _tournamentEntryService.GetAllByTournamentIdAsync(tournamentId);

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
                _logger.LogError(ex, "Unexpected error during GetAll for {TournamentId}", tournamentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _tournamentEntryService.GetByIdAsync(id);

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
        public async Task<IActionResult> CreateAsync([FromBody] CreateTournamentEntryRequest createTournamentEntryRequest)
        {
            try
            {
                var result = await _tournamentEntryService.CreateAsync(createTournamentEntryRequest);

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
                _logger.LogError(ex, "Unexpected error during Create for {TournamentId}", createTournamentEntryRequest.TournamentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateTournamentEntryRequest updateTournamentEntryRequest)
        {
            try
            {
                updateTournamentEntryRequest.Id = id;
                var result = await _tournamentEntryService.UpdateAsync(updateTournamentEntryRequest);

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
                _logger.LogError(ex, "Unexpected error during Update for {Id}", updateTournamentEntryRequest.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _tournamentEntryService.DeleteAsync(id);

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