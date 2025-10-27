using System.Threading.Tasks;

namespace Billing.Data;

public interface IBillingDbSchemaMigrator
{
    Task MigrateAsync();
}
