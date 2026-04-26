using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.Notices;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public EditModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public Notice Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Notices.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();
        var entity = await _db.Notices.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Date = Item.Date;
        entity.Type = Item.Type;
        entity.Title = Item.Title;
        entity.Body = Item.Body;
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Notice updated.";
        return Redirect("/admin/notices");
    }
}
