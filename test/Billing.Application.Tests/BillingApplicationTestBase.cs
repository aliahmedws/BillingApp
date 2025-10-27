using Volo.Abp.Modularity;

namespace Billing;

public abstract class BillingApplicationTestBase<TStartupModule> : BillingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
