using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.HomeStats;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<HomeStat> Items { get; set; } = PaginatedList<HomeStat>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.HomeStats.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(s =>
                s.Label.Contains(q) ||
                s.Value.Contains(q));
        }
        query = query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Id);
        Items = await PaginatedList<HomeStat>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.HomeStats.FindAsync(id);
        if (item is not null)
        {
            _db.HomeStats.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Stat deleted.";
        }
        return Redirect("/admin/home-stats");
    }
}
