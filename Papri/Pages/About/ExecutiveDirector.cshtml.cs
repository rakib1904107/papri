using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages.About;

[AllowAnonymous]
public class ExecutiveDirectorModel : PageModel
{
    public void OnGet() { }
}
