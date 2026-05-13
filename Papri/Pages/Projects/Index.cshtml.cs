using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Projects;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public List<Project> Items { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _db.Projects
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Id)
            .ToListAsync();
    }
}
