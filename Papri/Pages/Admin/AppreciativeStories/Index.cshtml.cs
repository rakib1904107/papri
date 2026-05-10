using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.AppreciativeStories;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public IndexModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    public PaginatedList<AppreciativeStory> Items { get; set; } = PaginatedList<AppreciativeStory>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.AppreciativeStories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(s => s.Name.Contains(q) || s.Story.Contains(q));
        query = query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Id);
        Items = await PaginatedList<AppreciativeStory>.CreateAsync(query, pageNumber ?? 1, 8);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.AppreciativeStories.FindAsync(id);
        if (item is not null)
        {
            _files.Delete(item.ImagePath);
            _db.AppreciativeStories.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Appreciative story deleted.";
        }
        return Redirect("/admin/appreciative-stories");
    }
}
