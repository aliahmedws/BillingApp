using Billing.Samples;
using Xunit;

namespace Billing.EntityFrameworkCore.Domains;

[Collection(BillingTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<BillingEntityFrameworkCoreTestModule>
{

}
