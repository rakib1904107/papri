using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Experiences;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<Experience> Items { get; set; } = PaginatedList<Experience>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.Experiences.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(e =>
                e.ProjectName.Contains(q) ||
                (e.DonorPartner != null && e.DonorPartner.Contains(q)));
        }
        query = query.OrderBy(e => e.DisplayOrder).ThenBy(e => e.Id);
        Items = await PaginatedList<Experience>.CreateAsync(query, pageNumber ?? 1, 5);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Experiences.FindAsync(id);
        if (item is not null)
        {
            _db.Experiences.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Experience deleted.";
        }
        return Redirect("/admin/experiences");
    }
}
