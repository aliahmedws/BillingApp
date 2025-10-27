using Volo.Abp.Modularity;

namespace Billing;

/* Inherit from this class for your domain layer tests. */
public abstract class BillingDomainTestBase<TStartupModule> : BillingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
