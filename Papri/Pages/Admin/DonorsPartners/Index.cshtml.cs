using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.DonorsPartners;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<DonorPartner> Items { get; set; } = PaginatedList<DonorPartner>.Empty();
    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        var query = _db.DonorPartners.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(d => d.Name.Contains(q));
        query = query.OrderBy(d => d.Category).ThenBy(d => d.DisplayOrder).ThenBy(d => d.Id);
        Items = await PaginatedList<DonorPartner>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.DonorPartners.FindAsync(id);
        if (item is not null)
        {
            _db.DonorPartners.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Donor / Partner deleted.";
        }
        return Redirect("/admin/donors-partners");
    }
}
