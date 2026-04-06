namespace Examples.Web.Authentication;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task UpdateTimestampAsync(string email, DateTimeOffset? timestamp = null, CancellationToken cancellationToken = default);

    Task<bool> ValidateLastChangedAsync(string email, string lastChanged, CancellationToken cancellationToken = default);
}
