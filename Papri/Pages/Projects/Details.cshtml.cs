using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Projects;

[AllowAnonymous]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public DetailsModel(ApplicationDbContext db) => _db = db;

    public Project Item { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Projects.FindAsync(id);
        if (entity is null || !entity.IsActive) return NotFound();
        Item = entity;
        return Page();
    }
}
