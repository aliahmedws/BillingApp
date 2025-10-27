using Microsoft.Extensions.Localization;
using Billing.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Billing;

[Dependency(ReplaceServices = true)]
public class BillingBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<BillingResource> _localizer;

    public BillingBrandingProvider(IStringLocalizer<BillingResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
