using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examples.Web.Authentication.Cookie.Pages;

public class PrivacyModel : PageModel
{
    private readonly IUserRepository _repository;

    public string? StatusMessage { get; set; }

    public PrivacyModel(IUserRepository repository)
    {
        _repository = repository;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostUpdateAccountAsync()
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
        {
            return NotFound();
        }

        // This simulates a backend update.
        // In a real application, this could be triggered by an API call to update the user's profile, change their password, etc.
        await _repository.UpdateTimestampAsync(email);
        StatusMessage = "Updated successfully.";

        return Page();
    }
}

