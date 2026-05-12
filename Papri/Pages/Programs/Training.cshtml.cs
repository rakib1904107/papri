using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages.Programs;

[AllowAnonymous]
public class TrainingModel : PageModel
{
    public void OnGet() { }
}
