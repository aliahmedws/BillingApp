using Volo.Abp;

namespace Billing.MeterDocuments;

public class MeterDocumentEmptyException : BusinessException
{
    public MeterDocumentEmptyException() : base(BillingDomainErrorCodes.MeterDocumentEmpty) { }
}
