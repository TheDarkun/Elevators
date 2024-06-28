using Elevators.Models;

namespace Elevators.Database;

public interface IDataAccess
{
    /// <summary>
    /// Creates or updates a user model in the database asynchronously.
    /// </summary>
    /// <param name="userModel">The user model to create or update.</param>
    public Task CreateOrUpdateUserModelAsync(UserModel userModel);
    
    /// <summary>
    /// Retrieves a user model by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user model if found, or null.</returns>
    public Task<UserModel?> GetUserModelByIdAsync(ulong id);
}