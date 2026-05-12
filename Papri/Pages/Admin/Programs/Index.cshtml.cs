using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Programs;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public IndexModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    public PaginatedList<ProgramItem> Items { get; set; } = PaginatedList<ProgramItem>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Programs.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(p =>
                p.Title.Contains(q) ||
                (p.DonorsPartner != null && p.DonorsPartner.Contains(q)) ||
                (p.WorkingArea != null && p.WorkingArea.Contains(q)));
        }
        query = query.OrderBy(p => p.Type).ThenBy(p => p.DisplayOrder).ThenBy(p => p.Id);
        Items = await PaginatedList<ProgramItem>.CreateAsync(query, pageNumber ?? 1, 8);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Programs.FindAsync(id);
        if (item is not null)
        {
            _files.Delete(item.ImagePath);
            _db.Programs.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Program deleted.";
        }
        return Redirect("/admin/programs");
    }
}
