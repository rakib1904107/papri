using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.About;

[AllowAnonymous]
public class ExperienceModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public ExperienceModel(ApplicationDbContext db) => _db = db;

    public List<Experience> Items { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _db.Experiences
            .Where(e => e.IsActive)
            .OrderBy(e => e.DisplayOrder)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }
}
