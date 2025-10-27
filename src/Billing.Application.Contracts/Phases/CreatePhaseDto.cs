using System.ComponentModel.DataAnnotations;

namespace Billing.Phases;

public class CreatePhaseDto
{
    [StringLength(PhaseConsts.MaxPhaseCodeLength)]
    public string? PhaseCode { get; set; }

    [Required]
    [StringLength(PhaseConsts.MaxPhaseNameLength)]
    public string PhaseName { get; set; } = string.Empty;

    [StringLength(PhaseConsts.MaxDescriptionLength)]
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
