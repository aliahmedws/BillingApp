using Billing.PlotDocuments;
using Billing.PlotTransferHistories;
using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.PlotTransferHistoryDocuments;

public class CreatePlotTransferHistoryDocumentDto
{
    public Guid PlotTransferHistoryId { get; set; }
    public PlotHistoryDocumentType PlotHistoryDT { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }

    [MaxLength(PlotTransferHistoryConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }
    public bool IsVerified { get; set; }
}
