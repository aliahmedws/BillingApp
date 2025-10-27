using Billing.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BillingController : AbpControllerBase
{
    protected BillingController()
    {
        LocalizationResource = typeof(BillingResource);
    }
}
