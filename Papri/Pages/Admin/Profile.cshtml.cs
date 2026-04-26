using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages.Admin;

public class ProfileModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ProfileModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty] public EmailInput Email { get; set; } = new();
    [BindProperty] public PasswordInput Password { get; set; } = new();

    public string CurrentEmail { get; set; } = string.Empty;

    [TempData] public string? StatusMessage { get; set; }
    [TempData] public string? ErrorMessage { get; set; }

    public class EmailInput
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "New email")]
        public string NewEmail { get; set; } = string.Empty;
    }

    public class PasswordInput
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Redirect("/admin");
        CurrentEmail = user.Email ?? string.Empty;
        Email.NewEmail = CurrentEmail;
        return Page();
    }

    private void ClearStateExcept(string prefix)
    {
        foreach (var key in ModelState.Keys.ToList())
        {
            if (!key.StartsWith(prefix + ".", StringComparison.OrdinalIgnoreCase) &&
                !key.Equals(prefix, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.Remove(key);
            }
        }
    }

    public async Task<IActionResult> OnPostUpdateEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Redirect("/admin");

        ClearStateExcept("Email");
        CurrentEmail = user.Email ?? string.Empty;

        if (!ModelState.IsValid) return Page();

        if (string.Equals(Email.NewEmail, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            StatusMessage = "Email is unchanged.";
            return Redirect("/admin/profile");
        }

        var setEmail = await _userManager.SetEmailAsync(user, Email.NewEmail);
        if (!setEmail.Succeeded)
        {
            ErrorMessage = string.Join(" ", setEmail.Errors.Select(e => e.Description));
            return Redirect("/admin/profile");
        }

        var setUser = await _userManager.SetUserNameAsync(user, Email.NewEmail);
        if (!setUser.Succeeded)
        {
            ErrorMessage = string.Join(" ", setUser.Errors.Select(e => e.Description));
            return Redirect("/admin/profile");
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Email updated successfully.";
        return Redirect("/admin/profile");
    }

    public async Task<IActionResult> OnPostChangePasswordAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Redirect("/admin");

        ClearStateExcept("Password");
        CurrentEmail = user.Email ?? string.Empty;

        if (string.IsNullOrEmpty(Password.CurrentPassword))
            ModelState.AddModelError("Password.CurrentPassword", "Current password is required.");
        if (string.IsNullOrEmpty(Password.NewPassword) || Password.NewPassword.Length < 8)
            ModelState.AddModelError("Password.NewPassword", "New password must be at least 8 characters.");
        if (Password.NewPassword != Password.ConfirmPassword)
            ModelState.AddModelError("Password.ConfirmPassword", "Passwords do not match.");

        if (!ModelState.IsValid) return Page();

        var changeResult = await _userManager.ChangePasswordAsync(user, Password.CurrentPassword, Password.NewPassword);
        if (!changeResult.Succeeded)
        {
            ErrorMessage = string.Join(" ", changeResult.Errors.Select(e => e.Description));
            return Redirect("/admin/profile");
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Password changed successfully.";
        return Redirect("/admin/profile");
    }
}
