using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.About;

[AllowAnonymous]
public class DonorsPartnersModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public DonorsPartnersModel(ApplicationDbContext db) => _db = db;

    public List<DonorPartner> Ongoing { get; private set; } = new();
    public List<DonorPartner> Previous { get; private set; } = new();
    public List<DonorPartner> Networks { get; private set; } = new();

    public async Task OnGetAsync()
    {
        var items = await _db.DonorPartners
            .Where(d => d.IsActive)
            .OrderBy(d => d.DisplayOrder)
            .ThenBy(d => d.Id)
            .ToListAsync();

        Ongoing  = items.Where(d => d.Category == DonorPartnerCategory.Ongoing).ToList();
        Previous = items.Where(d => d.Category == DonorPartnerCategory.Previous).ToList();
        Networks = items.Where(d => d.Category == DonorPartnerCategory.Network).ToList();
    }
}
