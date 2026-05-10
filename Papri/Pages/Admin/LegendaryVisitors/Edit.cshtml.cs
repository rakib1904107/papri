using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.LegendaryVisitors;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public EditModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public LegendaryVisitor Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.LegendaryVisitors.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();
        var entity = await _db.LegendaryVisitors.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Name = Item.Name;
        entity.Designation = Item.Designation;
        entity.CountryInstitution = Item.CountryInstitution;
        entity.Year = Item.Year;
        entity.DisplayOrder = Item.DisplayOrder;
        entity.IsActive = Item.IsActive;
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Visitor updated.";
        return Redirect("/admin/legendary-visitors");
    }
}
