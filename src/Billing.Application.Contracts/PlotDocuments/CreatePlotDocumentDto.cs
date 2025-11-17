using Billing.PlotInfos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.PlotDocuments;

public class CreatePlotDocumentDto
{
    [Required]
    public Guid PlotInfoId { get; set; }

    [StringLength(PlotInfoConsts.MaxRemarksLength)]
    public string? Description { get; set; }

    [StringLength(PlotInfoConsts.MaxRemarksLength)]
    public string? DocumentNumber { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    [Required]
    public PlotDocumentType PlotDocumentType { get; set; }
    public bool IsVerified { get; set; } = false;
}
