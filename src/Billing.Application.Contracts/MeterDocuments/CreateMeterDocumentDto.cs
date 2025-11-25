using System;

namespace Billing.MeterDocuments;

public class CreateMeterDocumentDto
{
    public Guid MeterId { get; set; }
    public MeterDocumentType Type { get; set; }
    public string? Description { get; set; }
}
