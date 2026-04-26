using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Headlines;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<Headline> Items { get; set; } = PaginatedList<Headline>.Empty();

    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Headlines.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(h => h.Text.Contains(q));
        query = query.OrderByDescending(h => h.CreatedAt).ThenByDescending(h => h.Id);

        Items = await PaginatedList<Headline>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Headlines.FindAsync(id);
        if (item is not null)
        {
            _db.Headlines.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Headline deleted.";
        }
        return Redirect("/admin/headlines");
    }
}
