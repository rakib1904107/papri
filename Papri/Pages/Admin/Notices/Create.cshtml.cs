using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.Notices;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CreateModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public Notice Item { get; set; } = new() { Date = DateTime.UtcNow };

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.Notices.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Notice created.";
        return Redirect("/admin/notices");
    }
}
