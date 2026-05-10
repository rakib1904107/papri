using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class LegendaryVisitor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(200)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "Designation")]
    public string? Designation { get; set; }

    [StringLength(300)]
    [Display(Name = "Country / Institution")]
    public string? CountryInstitution { get; set; }

    [StringLength(50)]
    [Display(Name = "Year")]
    public string? Year { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
