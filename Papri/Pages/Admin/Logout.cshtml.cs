using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages.Admin;

public class LogoutModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutModel(SignInManager<IdentityUser> signInManager) => _signInManager = signInManager;

    public IActionResult OnGet() => Redirect("/admin/dashboard");

    public async Task<IActionResult> OnPostAsync()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/admin");
    }
}
