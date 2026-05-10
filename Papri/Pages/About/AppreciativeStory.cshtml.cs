using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.About;

[AllowAnonymous]
public class AppreciativeStoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AppreciativeStoryModel(ApplicationDbContext db) => _db = db;

    public List<AppreciativeStory> Items { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _db.AppreciativeStories
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.Id)
            .ToListAsync();
    }
}
