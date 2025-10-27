using Billing.Samples;
using Xunit;

namespace Billing.EntityFrameworkCore.Applications;

[Collection(BillingTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<BillingEntityFrameworkCoreTestModule>
{

}
