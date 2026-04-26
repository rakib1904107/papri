using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class JobOpportunity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Job title is required.")]
    [StringLength(250)]
    [Display(Name = "Job title")]
    public string JobTitle { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Job posted")]
    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.Date)]
    [Display(Name = "Application deadline")]
    public DateTime Deadline { get; set; } = DateTime.UtcNow.AddDays(14);

    [Required]
    [StringLength(500)]
    [Display(Name = "Circular (PDF)")]
    public string CircularPath { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
