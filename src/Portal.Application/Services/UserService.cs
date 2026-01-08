using Microsoft.Extensions.Logging;
using Portal.Core.Interfaces;
using Portal.Core.Results;
using Portal.Domain.Entities;

namespace Portal.Application.Services;

/// <summary>
/// User service implementation with business logic
/// </summary>
public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IRepository<User> userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<User>> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with ID: {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                return Result<User>.Failure("User not found", $"No user exists with ID: {id}");
            }

            _logger.LogInformation("Successfully retrieved user with ID: {UserId}", id);
            return Result<User>.Success(user, "User retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user with ID: {UserId}", id);
            return Result<User>.Failure("Error retrieving user", ex.Message);
        }
    }

    public async Task<Result<IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all users");

            var users = await _userRepository.GetAllAsync(cancellationToken);
            var userList = users.ToList();

            _logger.LogInformation("Successfully retrieved {UserCount} users", userList.Count);
            return Result<IEnumerable<User>>.Success(userList, $"Retrieved {userList.Count} users successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all users");
            return Result<IEnumerable<User>>.Failure("Error retrieving users", ex.Message);
        }
    }

    public async Task<Result<User>> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user with email: {Email}", user.Email);

            // Check if user with email already exists
            var existingUsers = await _userRepository.FindAsync(u => u.Email == user.Email, cancellationToken);
            if (existingUsers.Any())
            {
                _logger.LogWarning("User with email {Email} already exists", user.Email);
                return Result<User>.Failure("User already exists", $"A user with email {user.Email} already exists");
            }

            var createdUser = await _userRepository.CreateAsync(user, cancellationToken);

            _logger.LogInformation("Successfully created user with ID: {UserId} and email: {Email}", 
                createdUser.Id, createdUser.Email);

            return Result<User>.Success(createdUser, "User created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user with email: {Email}", user.Email);
            return Result<User>.Failure("Error creating user", ex.Message);
        }
    }

    public async Task<Result> UpdateUserAsync(string id, User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (existingUser == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                return Result.Failure("User not found", $"No user exists with ID: {id}");
            }

            user.Id = id;
            var updated = await _userRepository.UpdateAsync(id, user, cancellationToken);

            if (!updated)
            {
                _logger.LogWarning("Failed to update user with ID: {UserId}", id);
                return Result.Failure("Update failed", "Failed to update user");
            }

            _logger.LogInformation("Successfully updated user with ID: {UserId}", id);
            return Result.Success("User updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {UserId}", id);
            return Result.Failure("Error updating user", ex.Message);
        }
    }

    public async Task<Result> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);

            var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (existingUser == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                return Result.Failure("User not found", $"No user exists with ID: {id}");
            }

            var deleted = await _userRepository.SoftDeleteAsync(id, cancellationToken);

            if (!deleted)
            {
                _logger.LogWarning("Failed to delete user with ID: {UserId}", id);
                return Result.Failure("Delete failed", "Failed to delete user");
            }

            _logger.LogInformation("Successfully deleted user with ID: {UserId}", id);
            return Result.Success("User deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", id);
            return Result.Failure("Error deleting user", ex.Message);
        }
    }
}
