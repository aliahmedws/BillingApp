using Volo.Abp;

namespace Billing.MeterDocuments;

public class DocumentEmptyException : BusinessException
{
    public DocumentEmptyException() : base(BillingDomainErrorCodes.DocumentEmpty) { }
}
