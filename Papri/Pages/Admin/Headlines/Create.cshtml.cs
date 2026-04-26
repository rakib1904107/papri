using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.Headlines;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CreateModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public Headline Item { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        Item.CreatedAt = DateTime.UtcNow;
        _db.Headlines.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Headline created.";
        return Redirect("/admin/headlines");
    }
}
