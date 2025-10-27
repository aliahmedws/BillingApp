using Volo.Abp.Modularity;

namespace Billing;

[DependsOn(
    typeof(BillingApplicationModule),
    typeof(BillingDomainTestModule)
)]
public class BillingApplicationTestModule : AbpModule
{

}
