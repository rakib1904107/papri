using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.HomeStats;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public EditModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public HomeStat Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.HomeStats.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();
        var entity = await _db.HomeStats.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Label = Item.Label;
        entity.Value = Item.Value;
        entity.DisplayOrder = Item.DisplayOrder;
        entity.IsActive = Item.IsActive;
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Stat updated.";
        return Redirect("/admin/home-stats");
    }
}
