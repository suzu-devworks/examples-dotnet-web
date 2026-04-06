namespace Examples.Web.Authentication;

public class ApplicationUser
{
    public required string Email { get; init; }
    public required string FullName { get; init; }

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    // LastChanged might be checked by the update date or by the revision.
    public string LastChanged => UpdatedAt.ToString("o");

    public bool ValidatePassword(string password)
    {
        // NOTE: DO NOT USE THIS IMPLEMENTATION. THIS IS FOR DEMO PURPOSE ONLY
        // Write your implementation here and return true or false depending on the validation.
        _ = password; // To avoid the warning of unused parameter.
        return true;
    }
}
