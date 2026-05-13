using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public List<Slider> Slides { get; private set; } = new();
    public List<HomeStat> Stats { get; private set; } = new();
    public List<ProgramItem> ImpactPrograms { get; private set; } = new();
    public Project? LatestProject { get; private set; }

    public async Task OnGetAsync()
    {
        Slides = await _db.Sliders
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.Id)
            .ToListAsync();

        Stats = await _db.HomeStats
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.Id)
            .Take(4)
            .ToListAsync();

        ImpactPrograms = await _db.Programs
            .Where(p => p.IsActive && p.Type == ProgramType.Ongoing)
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.Id)
            .Take(4)
            .ToListAsync();

        LatestProject = await _db.Projects
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Id)
            .FirstOrDefaultAsync();
    }
}
