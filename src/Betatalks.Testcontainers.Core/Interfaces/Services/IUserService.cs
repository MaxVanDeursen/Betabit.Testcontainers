using Betatalks.Testcontainers.Core.Entities;
using Betatalks.Testcontainers.Core.Requests;
using Betatalks.Testcontainers.Core.Results;

namespace Betatalks.Testcontainers.Core.Interfaces.Services;
public interface IUserService
{
    public Task<OperationResult<IEnumerable<User>>> GetUsersAsync(CancellationToken cancellationToken = default);

    public Task<OperationResult<User>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<OperationResult<User>> AddUserAsync(UpsertUserRequest addRequest, CancellationToken cancellationToken = default);
}
