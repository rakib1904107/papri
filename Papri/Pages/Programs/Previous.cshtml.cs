using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Programs;

[AllowAnonymous]
public class PreviousModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public PreviousModel(ApplicationDbContext db) => _db = db;

    public List<ProgramItem> Items { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _db.Programs
            .Where(p => p.IsActive && p.Type == ProgramType.Previous)
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.Id)
            .ToListAsync();
    }
}
