using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Jobs;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public IndexModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    public PaginatedList<JobOpportunity> Items { get; set; } = PaginatedList<JobOpportunity>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.JobOpportunities.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(j => j.JobTitle.Contains(q));
        query = query.OrderByDescending(j => j.PostedDate).ThenByDescending(j => j.Id);
        Items = await PaginatedList<JobOpportunity>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.JobOpportunities.FindAsync(id);
        if (item is not null)
        {
            _files.Delete(item.CircularPath);
            _db.JobOpportunities.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Job opportunity deleted.";
        }
        return Redirect("/admin/jobs");
    }
}
