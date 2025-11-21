using Billing.MeterDocuments;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Billing.MeterInfos;

public class MeterInfoDto : EntityDto<Guid>
{
    public string MeterNo { get; set; } = string.Empty;
    public MeterType MeterType { get; set; }
    public MeterCategory MeterCategory { get; set; }
    public MeterStatus MeterStatus { get; set; }
    public DateTime InstallationDate { get; set; }
    public decimal InitialReading { get; set; }
    public Guid PhaseId { get; set; }
    public Guid BlockId { get; set; }
    public string? PhaseName { get; set; }
    public Guid PlotId { get; set; }
    public Guid MeterOwnerId { get; set; }
    public string MeterOwnerName { get; set; } = string.Empty; 
    public string? PlotNo { get; set; }
    public string? Remarks { get; set; }
    public List<MeterDocumentDto> MeterDocuments { get; set; } = new();

}
