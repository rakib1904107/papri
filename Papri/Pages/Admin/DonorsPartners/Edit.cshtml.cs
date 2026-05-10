using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.DonorsPartners;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public EditModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public DonorPartner Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.DonorPartners.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();
        var entity = await _db.DonorPartners.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Name = Item.Name;
        entity.Category = Item.Category;
        entity.DisplayOrder = Item.DisplayOrder;
        entity.IsActive = Item.IsActive;
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Donor / Partner updated.";
        return Redirect("/admin/donors-partners");
    }
}
