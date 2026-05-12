using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public enum ProgramType
{
    [Display(Name = "Ongoing")] Ongoing = 0,
    [Display(Name = "Previous")] Previous = 1
}

public class ProgramItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(300)]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Image")]
    public string? ImagePath { get; set; }

    [StringLength(300)]
    [Display(Name = "Donors / Partner")]
    public string? DonorsPartner { get; set; }

    [StringLength(300)]
    [Display(Name = "Target People")]
    public string? TargetPeople { get; set; }

    [StringLength(300)]
    [Display(Name = "Working Area")]
    public string? WorkingArea { get; set; }

    [StringLength(100)]
    [Display(Name = "Duration")]
    public string? Duration { get; set; }

    [Display(Name = "Goal")]
    public string? Goal { get; set; }

    [Display(Name = "Objective (one per line)")]
    public string? Objective { get; set; }

    [Display(Name = "Major Activities (one per line)")]
    public string? MajorActivities { get; set; }

    [Required]
    [Display(Name = "Type")]
    public ProgramType Type { get; set; } = ProgramType.Ongoing;

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    public IEnumerable<string> ActivityList() => SplitLines(MajorActivities);
    public IEnumerable<string> ObjectiveList() => SplitLines(Objective);

    private static IEnumerable<string> SplitLines(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return Array.Empty<string>();
        return text
            .Replace("\r\n", "\n")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
