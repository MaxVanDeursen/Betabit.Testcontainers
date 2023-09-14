using System.Net.Mail;
using Betatalks.Testcontainers.Core.Entities;
using Betatalks.Testcontainers.Core.Exceptions;
using Betatalks.Testcontainers.Core.Interfaces.Repositories;
using Betatalks.Testcontainers.Core.Interfaces.Services;
using Betatalks.Testcontainers.Core.Requests;
using Betatalks.Testcontainers.Core.Results;

namespace Betatalks.Testcontainers.Core.Services;

public class UserService : IUserService
{
    private IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<IEnumerable<User>>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return OperationResult.Success(await _repository.GetUsersAsync(cancellationToken).ConfigureAwait(false));
        }
        catch (Exception exception)
        {
            return OperationResult.Failure<IEnumerable<User>>(exception);
        }
    }

    public async Task<OperationResult<User>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User? user = null;
        try
        {
            user = await _repository.GetUserByIdAsync(id, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            return OperationResult.Failure<User>(exception);
        }

        if (user == null)
        {
            return OperationResult.Failure<User>(new NotFoundException(id, nameof(User)));
        }

        return OperationResult.Success(user);
    }

    public async Task<OperationResult<User>> AddUserAsync(UpsertUserRequest addRequest, CancellationToken cancellationToken = default)
    {
        if (addRequest == null)
        {
            return OperationResult.Failure<User>(new ArgumentNullException(nameof(addRequest)));
        }

        try
        {
            var user = new User(addRequest.Name, addRequest.UserName, new MailAddress(addRequest.Email), addRequest.DateOfBirth);
            var persistedUser = await _repository.AddUserAsync(user, cancellationToken).ConfigureAwait(false);
            return OperationResult.Success(persistedUser);
        }
        catch (ArgumentException exception)
        {
            return OperationResult.Failure<User>(exception);
        }
        catch (Exception exception)
        {
            return OperationResult.Failure<User>(exception);
        }
    }
}
