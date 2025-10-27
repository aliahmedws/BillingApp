using Volo.Abp.Modularity;

namespace Billing;

[DependsOn(
    typeof(BillingDomainModule),
    typeof(BillingTestBaseModule)
)]
public class BillingDomainTestModule : AbpModule
{

}
