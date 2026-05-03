using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages.About;

[AllowAnonymous]
public class OrganogramModel : PageModel
{
    public void OnGet() { }
}
