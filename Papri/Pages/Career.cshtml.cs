using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages;

[AllowAnonymous]
public class CareerModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CareerModel(ApplicationDbContext db) => _db = db;

    public List<JobOpportunity> OpenJobs { get; private set; } = new();

    public async Task OnGetAsync()
    {
        var today = DateTime.UtcNow.Date;
        OpenJobs = await _db.JobOpportunities
            .Where(j => j.Deadline.Date >= today)
            .OrderByDescending(j => j.PostedDate)
            .ThenByDescending(j => j.Id)
            .ToListAsync();
    }
}
