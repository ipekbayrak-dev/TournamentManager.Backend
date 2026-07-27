using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application.Dtos.Auth;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService, ILogger<AuthController> _logger) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest signUpRequest)
        {
            try
            {
                var result = await _authService.SignUpAsync(signUpRequest);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(new { Message = "User registered successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during SignUp for {Email}", signUpRequest.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest signInRequest)
        {
            try
            {
                var result = await _authService.SignInAsync(signInRequest);

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
                _logger.LogError(ex, "Unexpected error during SignIn for {Email}", signInRequest.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] TokenResponse tokenResponse)
        {
            try
            {
                var result = await _authService.RefreshAsync(tokenResponse);

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
                _logger.LogError(ex, "Unexpected error during token refresh {Token}", tokenResponse.RefreshToken);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            }
        }
    }
}