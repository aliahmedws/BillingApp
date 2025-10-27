using Billing.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Billing.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BillingEntityFrameworkCoreModule),
    typeof(BillingApplicationContractsModule)
)]
public class BillingDbMigratorModule : AbpModule
{
}
