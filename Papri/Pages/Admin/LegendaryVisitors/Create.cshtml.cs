using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.LegendaryVisitors;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CreateModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public LegendaryVisitor Item { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.LegendaryVisitors.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Visitor created.";
        return Redirect("/admin/legendary-visitors");
    }
}
