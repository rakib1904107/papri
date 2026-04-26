using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.News;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public IndexModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    public PaginatedList<Papri.Models.News> Items { get; set; } = PaginatedList<Papri.Models.News>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.News.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(n => n.Title.Contains(q) || n.Details.Contains(q));
        query = query.OrderByDescending(n => n.Date).ThenByDescending(n => n.Id);
        Items = await PaginatedList<Papri.Models.News>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.News.FindAsync(id);
        if (item is not null)
        {
            _files.Delete(item.ImagePath);
            _db.News.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "News item deleted.";
        }
        return Redirect("/admin/news");
    }
}
