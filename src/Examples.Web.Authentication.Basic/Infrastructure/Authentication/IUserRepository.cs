namespace Examples.Web.Authentication;

/// <summary>
/// NOTE: DO NOT USE THIS IMPLEMENTATION. THIS IS FOR DEMO PURPOSE ONLY
/// </summary>
public interface IUserRepository
{
    Task<User?> GetUserByUsername(string username);

    Task<IEnumerable<User>> GetUsers();

}
