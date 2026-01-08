using Portal.Core.Results;
using Portal.Domain.Entities;

namespace Portal.Application.Services;

/// <summary>
/// User service interface for business operations
/// </summary>
public interface IUserService
{
    Task<Result<User>> GetUserByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<User>> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<Result> UpdateUserAsync(string id, User user, CancellationToken cancellationToken = default);
    Task<Result> DeleteUserAsync(string id, CancellationToken cancellationToken = default);
}
