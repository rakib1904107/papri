using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.Videos;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CreateModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public Video Item { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        Item.YouTubeId = ExtractId(Item.YouTubeId);
        Item.AddedAt = DateTime.UtcNow;
        _db.Videos.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Video added.";
        return Redirect("/admin/videos");
    }

    private static string ExtractId(string input)
    {
        input = input.Trim();
        if (Uri.TryCreate(input, UriKind.Absolute, out var uri))
        {
            var v = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            if (v.TryGetValue("v", out var id)) return id.ToString();
            var path = uri.AbsolutePath.Trim('/');
            if (!string.IsNullOrEmpty(path)) return path.Split('/').Last();
        }
        return input;
    }
}
