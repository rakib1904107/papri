using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class Headline
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Headline text is required.")]
    [StringLength(300)]
    [Display(Name = "Headline")]
    public string Text { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
