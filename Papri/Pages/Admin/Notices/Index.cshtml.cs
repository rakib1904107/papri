using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Notices;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<Notice> Items { get; set; } = PaginatedList<Notice>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Notices.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(n => n.Title.Contains(q) || (n.Body != null && n.Body.Contains(q)));
        query = query.OrderByDescending(n => n.Date).ThenByDescending(n => n.Id);
        Items = await PaginatedList<Notice>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Notices.FindAsync(id);
        if (item is not null)
        {
            _db.Notices.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Notice deleted.";
        }
        return Redirect("/admin/notices");
    }
}
