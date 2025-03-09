using AspNetCore.Authentication.Basic;

namespace Examples.Web.Authentication.Basic;

public class BasicUserValidationService : IBasicUserValidationService
{
    private readonly IUserRepository _repository;
    private readonly ILogger<BasicUserValidationService> _logger;

    public BasicUserValidationService(IUserRepository repository, ILogger<BasicUserValidationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> IsValidAsync(string username, string password)
    {
        try
        {
            // NOTE: DO NOT USE THIS IMPLEMENTATION. THIS IS FOR DEMO PURPOSE ONLY
            // Write your implementation here and return true or false depending on the validation..
            var user = await _repository.GetUserByUsername(username);
            var isValid = user != null && user.Password == password;

            return isValid;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Catch the exception: {message}", e.Message);
            throw;
        }
    }
}