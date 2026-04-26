using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Sliders;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public IndexModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    public PaginatedList<Slider> Items { get; set; } = PaginatedList<Slider>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Sliders.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(s => s.Caption != null && s.Caption.Contains(q));
        query = query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Id);
        Items = await PaginatedList<Slider>.CreateAsync(query, pageNumber ?? 1, 12);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Sliders.FindAsync(id);
        if (item is not null)
        {
            _files.Delete(item.ImagePath);
            _db.Sliders.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Slider deleted.";
        }
        return Redirect("/admin/sliders");
    }
}
