using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Projects;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public IndexModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    public PaginatedList<Project> Items { get; set; } = PaginatedList<Project>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Projects.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Title.Contains(q) || p.Details.Contains(q));
        query = query.OrderByDescending(p => p.Date).ThenByDescending(p => p.Id);
        Items = await PaginatedList<Project>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Projects.FindAsync(id);
        if (item is not null)
        {
            _files.Delete(item.ImagePath);
            _db.Projects.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Project deleted.";
        }
        return Redirect("/admin/projects");
    }
}
