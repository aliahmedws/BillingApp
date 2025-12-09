using Billing.MaintenanceBills;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace Billing.MaintenancePaymentHistories;

public class MaintenancePaymentExcelDto
{
    public Guid MaintenanceBillId { get; set; }
    public string TransactionId { get; set; } = default!;
    public decimal PaymentReceived { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Method { get; set; } = default!;
}

public class MaintenancePaymentFullExcelDto
{
    public Guid MaintenanceBillId { get; set; }
    public string ConsumerName { get; set; } = default!;
    public string Plot { get; set; } = default!;
    public string BillingMonth { get; set; } = default!;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public decimal WaterCharges { get; set; }
    public decimal SecurityCharges { get; set; }
    public decimal Arrears { get; set; }
    public decimal OtherCharges { get; set; }
    public decimal RefundOrBenefit { get; set; }
    public decimal AnyOtherWorkCharges { get; set; }
    public decimal LatePaymentSurcharge { get; set; }
    public decimal PaymentBeforeDueDate { get; set; }
    public decimal PayableAfterDueDate { get; set; }

    public string Status { get; set; }

    // Payment History Fields
    public string TransactionId { get; set; }
    public decimal PaymentReceived { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string Method { get; set; }

    public static Dictionary<string, string> GetHeaderMap(IStringLocalizer localizer)
    {
        return new Dictionary<string, string>
    {
        { nameof(MaintenancePaymentFullExcelDto.MaintenanceBillId), localizer["MaintenanceBillId"] },
        { nameof(MaintenancePaymentFullExcelDto.ConsumerName), localizer["Consumer"] },
        { nameof(MaintenancePaymentFullExcelDto.Plot), localizer["Plot"] },
        { nameof(MaintenancePaymentFullExcelDto.BillingMonth), localizer["BillingMonth"] },
        { nameof(MaintenancePaymentFullExcelDto.IssueDate), localizer["IssueDate"] },
        { nameof(MaintenancePaymentFullExcelDto.DueDate), localizer["DueDate"] },

        { nameof(MaintenancePaymentFullExcelDto.WaterCharges), localizer["WaterCharges"] },
        { nameof(MaintenancePaymentFullExcelDto.SecurityCharges), localizer["SecurityCharges"] },
        { nameof(MaintenancePaymentFullExcelDto.Arrears), localizer["Arrears"] },
        { nameof(MaintenancePaymentFullExcelDto.OtherCharges), localizer["OtherCharges"] },
        { nameof(MaintenancePaymentFullExcelDto.RefundOrBenefit), localizer["RefundOrBenefit"] },
        { nameof(MaintenancePaymentFullExcelDto.AnyOtherWorkCharges), localizer["AnyOtherWorkCharges"] },
        { nameof(MaintenancePaymentFullExcelDto.LatePaymentSurcharge), localizer["LatePaymentSurcharge"] },
        { nameof(MaintenancePaymentFullExcelDto.PaymentBeforeDueDate), localizer["PaymentBeforeDueDate"] },
        { nameof(MaintenancePaymentFullExcelDto.PayableAfterDueDate), localizer["PayableAfterDueDate"] },
        { nameof(MaintenancePaymentFullExcelDto.Status), localizer["Status"] },

        { nameof(MaintenancePaymentFullExcelDto.TransactionId), localizer["TransactionId"] },
        { nameof(MaintenancePaymentFullExcelDto.PaymentReceived), localizer["PaymentReceived"] },
        { nameof(MaintenancePaymentFullExcelDto.PaymentDate), localizer["PaymentDate"] },
        { nameof(MaintenancePaymentFullExcelDto.Method), localizer["Method"] },
    };
    }
}


