using System;

namespace Billing.Phases;

public class PhaseLookUp
{
    public Guid Id { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public string PhaseCode { get; set; } = string.Empty;
}
