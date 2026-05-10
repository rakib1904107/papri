using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.LegendaryVisitors;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<LegendaryVisitor> Items { get; set; } = PaginatedList<LegendaryVisitor>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.LegendaryVisitors.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(v =>
                v.Name.Contains(q) ||
                (v.Designation != null && v.Designation.Contains(q)) ||
                (v.CountryInstitution != null && v.CountryInstitution.Contains(q)));
        }
        query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Id);
        Items = await PaginatedList<LegendaryVisitor>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.LegendaryVisitors.FindAsync(id);
        if (item is not null)
        {
            _db.LegendaryVisitors.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Visitor deleted.";
        }
        return Redirect("/admin/legendary-visitors");
    }
}
