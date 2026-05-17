using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class HomeStat
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Label is required.")]
    [StringLength(100)]
    [Display(Name = "Label")]
    public string Label { get; set; } = string.Empty;

    [Required(ErrorMessage = "Value is required.")]
    [StringLength(30)]
    [Display(Name = "Value")]
    public string Value { get; set; } = string.Empty;

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
