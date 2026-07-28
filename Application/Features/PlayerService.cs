using FluentValidation;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Player;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Application.Features
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IValidator<CreatePlayerRequest> _createValidator;
        public PlayerService(IPlayerRepository playerRepository, IValidator<CreatePlayerRequest> createValidator)
        {
            _playerRepository = playerRepository;
            _createValidator = createValidator;
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

        public async Task<Result<PlayerResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var player = await _playerRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (player is null)
            {
                return Result<PlayerResponse>.Success(null);
            }

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
    }
}