using Betatalks.Testcontainers.Core.Entities;
using Betatalks.Testcontainers.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Betatalks.Testcontainers.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
    private TestcontainersContext _context;

    public UserRepository(TestcontainersContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return user;
    }
}
