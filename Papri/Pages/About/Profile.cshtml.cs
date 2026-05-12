using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.About;

[AllowAnonymous]
public class ProfileModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public ProfileModel(ApplicationDbContext db) => _db = db;

    public List<DonorPartner> OngoingDonors { get; private set; } = new();
    public List<DonorPartner> PreviousDonors { get; private set; } = new();
    public List<ProgramItem> OngoingPrograms { get; private set; } = new();
    public List<Experience> Experiences { get; private set; } = new();

    public async Task OnGetAsync()
    {
        var donors = await _db.DonorPartners
            .Where(d => d.IsActive && d.Category != DonorPartnerCategory.Network)
            .OrderBy(d => d.DisplayOrder)
            .ThenBy(d => d.Id)
            .ToListAsync();
        OngoingDonors = donors.Where(d => d.Category == DonorPartnerCategory.Ongoing).ToList();
        PreviousDonors = donors.Where(d => d.Category == DonorPartnerCategory.Previous).ToList();

        OngoingPrograms = await _db.Programs
            .Where(p => p.IsActive && p.Type == ProgramType.Ongoing)
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.Id)
            .ToListAsync();

        Experiences = await _db.Experiences
            .Where(e => e.IsActive)
            .OrderBy(e => e.DisplayOrder)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }
}
