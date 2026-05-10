using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.Experiences;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public EditModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public Experience Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Experiences.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();
        var entity = await _db.Experiences.FindAsync(id);
        if (entity is null) return NotFound();
        entity.ProjectName = Item.ProjectName;
        entity.DonorPartner = Item.DonorPartner;
        entity.Duration = Item.Duration;
        entity.Activities = Item.Activities;
        entity.DisplayOrder = Item.DisplayOrder;
        entity.IsActive = Item.IsActive;
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Experience updated.";
        return Redirect("/admin/experiences");
    }
}
