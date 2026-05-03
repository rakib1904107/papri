using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages;

[AllowAnonymous]
public class FeedbackModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public FeedbackModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public string Name { get; set; } = string.Empty;
    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Message { get; set; } = string.Empty;

    [TempData] public string? FeedbackStatus { get; set; }
    [TempData] public bool FeedbackSuccess { get; set; }

    public IActionResult OnGet() => Redirect("/");

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Message))
        {
            FeedbackSuccess = false;
            FeedbackStatus = "Please fill in all fields.";
            return Redirect(Request.Headers["Referer"].ToString() is { Length: > 0 } r ? r : "/");
        }

        var fb = new Feedback
        {
            Name = Name.Trim(),
            Email = Email.Trim(),
            Message = Message.Trim(),
            SubmittedAt = DateTime.UtcNow
        };
        _db.Feedbacks.Add(fb);
        await _db.SaveChangesAsync();

        FeedbackSuccess = true;
        FeedbackStatus = "Thanks for your feedback!";
        return Redirect(Request.Headers["Referer"].ToString() is { Length: > 0 } r2 ? r2 : "/");
    }
}
