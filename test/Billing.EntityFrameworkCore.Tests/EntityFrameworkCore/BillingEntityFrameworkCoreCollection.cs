using Xunit;

namespace Billing.EntityFrameworkCore;

[CollectionDefinition(BillingTestConsts.CollectionDefinitionName)]
public class BillingEntityFrameworkCoreCollection : ICollectionFixture<BillingEntityFrameworkCoreFixture>
{

}
