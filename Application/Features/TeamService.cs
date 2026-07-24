using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Team;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class TeamService : ITeamService
    {
        public Task<Result<TeamResponse>> CreateAsync(CreateTeamRequest createTeamRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<TeamResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TeamResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdateTeamRequest updateTeamRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}