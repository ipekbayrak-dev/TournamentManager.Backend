using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Player;
using TournamentManager.Application.Dtos.Team;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Application.Features
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        private static TeamResponse MapToResponse(Team team)
        {
            return new TeamResponse
            {
                Id = team.Id,
                Name = team.Name,
                Handle = team.Handle,
                Logo = team.Logo,
                Region = team.Region,
                Players = team.Players.Select(x => new PlayerResponse
                {
                    Id = x.Id,
                    Handle = x.Handle,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CountryCode = x.CountryCode,
                    Position = x.Position,
                    IsCaptain = x.IsCaptain,
                    SteamId = x.SteamId,
                    TeamId = x.TeamId
                }).ToList()
            };
        }
        public async Task<Result<TeamResponse>> CreateAsync(CreateTeamRequest createTeamRequest, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(createTeamRequest.Name))
            {
                return Result<TeamResponse>.Failure("Invalid team name.");
            }

            if (string.IsNullOrWhiteSpace(createTeamRequest.Handle))
            {
                return Result<TeamResponse>.Failure("Invalid team handle.");
            }

            var team = new Team
            {
                Name = createTeamRequest.Name,
                Handle = createTeamRequest.Handle,
                Logo = createTeamRequest.Logo,
                Region = createTeamRequest.Region
            };

            await _teamRepository.AddAsync(team);
            return Result<TeamResponse>.Success(MapToResponse(team));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (team is null)
            {
                return Result.Failure("Team not found");
            }

            await _teamRepository.DeleteAsync(team);

            return Result.Success();
        }

        public async Task<Result<ICollection<TeamResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetAllAsync(cancellationToken: cancellationToken);

            var response = team.Select(MapToResponse).ToList();

            return Result<ICollection<TeamResponse>>.Success(response);
        }

        public async Task<Result<TeamResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);
            
            if (team is null)
            {
                return Result<TeamResponse>.Failure("Invalid Id");
            }

            return Result<TeamResponse>.Success(MapToResponse(team));
        }

        public async Task<Result> UpdateAsync(UpdateTeamRequest updateTeamRequest, CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetAsync(x => x.Id == updateTeamRequest.Id, cancellationToken: cancellationToken);

            if (team is null)
            {
                return Result.Failure("Invalid Id");
            }

            team.Name = updateTeamRequest.Name;
            team.Handle = updateTeamRequest.Handle;
            team.Logo = updateTeamRequest.Logo;
            team.Region = updateTeamRequest.Region;

            await _teamRepository.UpdateAsync(team);

            return Result.Success();
        }
    }
}