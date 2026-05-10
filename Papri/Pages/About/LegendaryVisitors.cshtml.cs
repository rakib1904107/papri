using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.About;

[AllowAnonymous]
public class LegendaryVisitorsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public LegendaryVisitorsModel(ApplicationDbContext db) => _db = db;

    public List<LegendaryVisitor> Items { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _db.LegendaryVisitors
            .Where(v => v.IsActive)
            .OrderBy(v => v.DisplayOrder)
            .ThenBy(v => v.Id)
            .ToListAsync();
    }
}
