using Billing.Localization;
using Volo.Abp.Application.Services;

namespace Billing;

/* Inherit your application services from this class.
 */
public abstract class BillingAppService : ApplicationService
{
    protected BillingAppService()
    {
        LocalizationResource = typeof(BillingResource);
    }
}
