using Billing.ConsumerPersonalInfos;
using Billing.PlotTypes;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotInfos;

public class PlotInfoDto : EntityDto<Guid>
{
    public string PlotNo { get; set; } = string.Empty;
    public PlotType PlotType { get; set; }
    public string StreetNo { get; set; } = string.Empty;
    public Guid PlotSizeId { get; set; }
    public PlotStatus Status { get; set; }
    public Guid BlockId { get; set; }
    public Guid? ConsumerId { get; set; }
    public Guid PhaseId { get; set; }
    public string? Remarks { get; set; }

    // Related Names for Lookup Display
    public string? BlockName { get; set; }
    public string? PhaseName { get; set; }
    public string? PlotSizeName { get; set; }
    public string? ConsumerFullName { get; set; }
    //public ConsumerPersonalInfoDto Consumers { get; set; }
}
