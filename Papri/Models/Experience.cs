using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class Experience
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Project name is required.")]
    [StringLength(500)]
    [Display(Name = "Project Name")]
    public string ProjectName { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Donor / Partner")]
    public string? DonorPartner { get; set; }

    [StringLength(100)]
    [Display(Name = "Duration")]
    public string? Duration { get; set; }

    [Display(Name = "Activities (one per line)")]
    public string? Activities { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    public IEnumerable<string> ActivityList()
    {
        if (string.IsNullOrWhiteSpace(Activities)) return Array.Empty<string>();
        return Activities
            .Replace("\r\n", "\n")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
