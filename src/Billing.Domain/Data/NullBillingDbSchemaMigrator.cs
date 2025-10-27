using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Billing.Data;

/* This is used if database provider does't define
 * IBillingDbSchemaMigrator implementation.
 */
public class NullBillingDbSchemaMigrator : IBillingDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
