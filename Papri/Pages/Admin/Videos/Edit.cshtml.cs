using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.Videos;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public EditModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public Video Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Videos.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();
        var entity = await _db.Videos.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Title = Item.Title;
        entity.YouTubeId = ExtractId(Item.YouTubeId);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Video updated.";
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
