using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Team;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface ITeamService
    {
        public Task<Result<TeamResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<ICollection<TeamResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Result<TeamResponse>> CreateAsync(CreateTeamRequest createTeamRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdateTeamRequest updateTeamRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}