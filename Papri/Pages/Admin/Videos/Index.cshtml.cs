using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Videos;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<Video> Items { get; set; } = PaginatedList<Video>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Videos.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(v => v.Title.Contains(q) || v.YouTubeId.Contains(q));
        query = query.OrderByDescending(v => v.AddedAt).ThenByDescending(v => v.Id);
        Items = await PaginatedList<Video>.CreateAsync(query, pageNumber ?? 1, 12);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Videos.FindAsync(id);
        if (item is not null)
        {
            _db.Videos.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Video deleted.";
        }
        return Redirect("/admin/videos");
    }
}
