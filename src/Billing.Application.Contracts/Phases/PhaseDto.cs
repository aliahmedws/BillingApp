using System;
using Volo.Abp.Application.Dtos;

namespace Billing.Phases;

public class PhaseDto : EntityDto<Guid>
{
    public string PhaseCode { get; set; } = string.Empty;
    public string PhaseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
