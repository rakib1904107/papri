using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Admin.DonorsPartners;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CreateModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public DonorPartner Item { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.DonorPartners.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Donor / Partner created.";
        return Redirect("/admin/donors-partners");
    }
}
