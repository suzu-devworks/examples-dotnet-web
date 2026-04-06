namespace Examples.Web.Authentication.Repositories;

/// <summary>
/// NOTE: DO NOT USE THIS IMPLEMENTATION. THIS IS FOR DEMO PURPOSE ONLY
/// </summary>
internal class InMemoryUserRepository : IUserRepository
{
    private readonly List<ApplicationUser> _users = new()
    {
        new ApplicationUser { Email = "TestUser1@examples.local", FullName = "User One", UpdatedAt = DateTimeOffset.UtcNow },
        new ApplicationUser { Email = "TestUser2@examples.local", FullName = "User Two", UpdatedAt = DateTimeOffset.UtcNow },
        new ApplicationUser { Email = "TestUser3@examples.local", FullName = "User Three", UpdatedAt = DateTimeOffset.UtcNow},
        new ApplicationUser { Email = "TestUser4@examples.local", FullName = "User Four", UpdatedAt = DateTimeOffset.UtcNow }
    };

    public Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
    }

    public Task UpdateTimestampAsync(string email, DateTimeOffset? timestamp = null, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(u => u.Email == email);
        if (user is not null)
        {
            user.UpdatedAt = timestamp ?? DateTimeOffset.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task<bool> ValidateLastChangedAsync(string email, string lastChanged, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(u => u.Email == email && u.LastChanged == lastChanged);
        return Task.FromResult(user is not null);
    }
}
