using Betatalks.Testcontainers.Core.Entities;

namespace Betatalks.Testcontainers.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default);

    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default);
}
