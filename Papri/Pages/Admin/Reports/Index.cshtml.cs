using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Reports;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public IndexModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    public PaginatedList<Report> Items { get; set; } = PaginatedList<Report>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Reports.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(r => r.Title.Contains(q));
        query = query.OrderByDescending(r => r.UploadedAt).ThenByDescending(r => r.Id);
        Items = await PaginatedList<Report>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Reports.FindAsync(id);
        if (item is not null)
        {
            _files.Delete(item.FilePath);
            _db.Reports.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Report deleted.";
        }
        return Redirect("/admin/reports");
    }
}
