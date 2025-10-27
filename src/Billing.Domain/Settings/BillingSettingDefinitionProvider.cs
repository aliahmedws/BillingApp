using Volo.Abp.Settings;

namespace Billing.Settings;

public class BillingSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(BillingSettings.MySetting1));
    }
}
