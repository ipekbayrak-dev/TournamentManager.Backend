using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Player;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Features
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IValidator<CreatePlayerRequest> _createValidator;
        private readonly UserManager<ApplicationUser> _userManager;
        public PlayerService(IPlayerRepository playerRepository, IValidator<CreatePlayerRequest> createValidator, UserManager<ApplicationUser> userManager)
        {
            _playerRepository = playerRepository;
            _createValidator = createValidator;
            _userManager = userManager;
        }
        private static PlayerResponse MapToResponse(Player player)
        {
            return new PlayerResponse
            {
                Id = player.Id,
                Handle = player.Handle,
                FirstName = player.FirstName,
                LastName = player.LastName,
                CountryCode = player.CountryCode,
                Status = player.Status,
                Position = player.Position,
                IsCaptain = player.IsCaptain,
                SteamId = player.SteamId,
                TeamId = player.TeamId
            };
        }
        public async Task<Result<PlayerResponse>> CreateAsync(CreatePlayerRequest createPlayerRequest, CancellationToken cancellationToken = default)
        {
            var validation = await _createValidator.ValidateAsync(createPlayerRequest, cancellationToken);

            if (!validation.IsValid)
                return Result<PlayerResponse>.Failure(validation.ToErrorMessage());

            var player = new Player
            {
                Handle = createPlayerRequest.Handle,
                FirstName = createPlayerRequest.FirstName,
                LastName = createPlayerRequest.LastName,
                CountryCode = createPlayerRequest.CountryCode,
                Status = PlayerStatus.Approved,
                Position = createPlayerRequest.Position,
                IsCaptain = createPlayerRequest.IsCaptain,
                SteamId = createPlayerRequest.SteamId,
                TeamId = createPlayerRequest.TeamId
            };

            await _playerRepository.AddAsync(player);

            return Result<PlayerResponse>.Success(MapToResponse(player));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (player is null)
            {
                return Result.Failure("Player not found");
            }

            await _playerRepository.DeleteAsync(player);

            return Result.Success();
        }

        public async Task<Result<ICollection<PlayerResponse>>> GetAllByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAllAsync(x => x.TeamId == teamId, cancellationToken: cancellationToken);

            var response = player.Select(MapToResponse).ToList();

            return Result<ICollection<PlayerResponse>>.Success(response);
        }

        public async Task<Result<ICollection<PlayerResponse>>> GetPendingPlayerAsync(CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAllAsync(x => x.Status == PlayerStatus.Pending, cancellationToken: cancellationToken);

            var response = player.Select(MapToResponse).ToList();

            return Result<ICollection<PlayerResponse>>.Success(response);
        }
        public async Task<Result<PlayerResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<PlayerResponse>.Failure("User not found");

            if (user.PlayerId is null)
                return Result<PlayerResponse>.Failure("No player profile found");

            var player = await _playerRepository.GetAsync(x => x.Id == user.PlayerId, cancellationToken: cancellationToken);
            if (player is null)
                return Result<PlayerResponse>.Failure("No player profile found");

            return Result<PlayerResponse>.Success(MapToResponse(player));
        }

        public async Task<Result<PlayerResponse>> CreateProfileAsync(CreatePlayerRequest createPlayerRequest, string userId, CancellationToken cancellationToken = default)
        {
            var validation = await _createValidator.ValidateAsync(createPlayerRequest, cancellationToken);

            if (!validation.IsValid)
            {
                return Result<PlayerResponse>.Failure(validation.ToErrorMessage());
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Result<PlayerResponse>.Failure("User not found");
            }

            if (user.PlayerId is not null)
            {
                return Result<PlayerResponse>.Failure("A player profile already exists for this account.");
            }

            var player = new Player
            {
                Handle = createPlayerRequest.Handle,
                FirstName = createPlayerRequest.FirstName,
                LastName = createPlayerRequest.LastName,
                CountryCode = createPlayerRequest.CountryCode,
                Status = PlayerStatus.Pending,
                Position = createPlayerRequest.Position,
                IsCaptain = createPlayerRequest.IsCaptain,
                SteamId = createPlayerRequest.SteamId,
                TeamId = createPlayerRequest.TeamId
            };

            await _playerRepository.AddAsync(player);

            user.PlayerId = player.Id;
            await _userManager.UpdateAsync(user);

            return Result<PlayerResponse>.Success(MapToResponse(player));

        }

        public async Task<Result<PlayerResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (player is null)
            {
                return Result<PlayerResponse>.Success(null);
            }

            return Result<PlayerResponse>.Success(MapToResponse(player));
        }

        public async Task<Result<ICollection<PlayerResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAllAsync(cancellationToken: cancellationToken);
            
            var response = player.Select(MapToResponse).ToList();
            
            return Result<ICollection<PlayerResponse>>.Success(response);
        }

        public async Task<Result<PlayerResponse>> ResubmitProfileAsync(CreatePlayerRequest request, string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Result<PlayerResponse>.Failure("User not found");
            if (user.PlayerId is null) return Result<PlayerResponse>.Failure("No player profile found");

            var player = await _playerRepository.GetAsync(x => x.Id == user.PlayerId, cancellationToken: cancellationToken);
            if (player is null) return Result<PlayerResponse>.Failure("Player not found");

            if (player.Status != PlayerStatus.Rejected)
                return Result<PlayerResponse>.Failure("Only rejected profiles can be resubmitted");

            var validation = await _createValidator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid) return Result<PlayerResponse>.Failure(validation.ToErrorMessage());

            player.Handle = request.Handle;
            player.FirstName = request.FirstName;
            player.LastName = request.LastName;
            player.CountryCode = request.CountryCode;
            player.Position = request.Position;
            player.IsCaptain = request.IsCaptain;
            player.SteamId = request.SteamId;
            player.TeamId = request.TeamId;
            player.Status = PlayerStatus.Pending;

            await _playerRepository.UpdateAsync(player);

            return Result<PlayerResponse>.Success(MapToResponse(player));
        }

        public async Task<Result> UpdateAsync(UpdatePlayerRequest updatePlayerRequest, CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAsync(x => x.Id == updatePlayerRequest.Id, cancellationToken: cancellationToken);

            if (player is null)
            {
                return Result.Failure("Invalid Id");
            }

            player.Handle = updatePlayerRequest.Handle;
            player.FirstName = updatePlayerRequest.FirstName;
            player.LastName = updatePlayerRequest.LastName;
            player.CountryCode = updatePlayerRequest.CountryCode;
            player.Position = updatePlayerRequest.Position;
            player.IsCaptain = updatePlayerRequest.IsCaptain;
            player.SteamId = updatePlayerRequest.SteamId;

            await _playerRepository.UpdateAsync(player);

            return Result.Success();
        }

        public async Task<Result> UpdatePlayerStatusAsync(Guid id, PlayerStatus playerStatus, CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (player is null)
            {
                return Result.Failure("Invalid Id");
            }

            player.Status = playerStatus;
            
            await _playerRepository.UpdateAsync(player);

            return Result.Success();
        }
    }
}