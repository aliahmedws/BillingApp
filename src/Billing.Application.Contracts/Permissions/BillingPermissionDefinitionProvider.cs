using Billing.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Billing.Permissions;

public class BillingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BillingPermissions.GroupName);

        var phasesPermission = myGroup.AddPermission(BillingPermissions.Phases.Default, L("Permission:Phases"));
        phasesPermission.AddChild(BillingPermissions.Phases.Create, L("Permission:Phases.Create"));
        phasesPermission.AddChild(BillingPermissions.Phases.Edit, L("Permission:Phases.Edit"));
        phasesPermission.AddChild(BillingPermissions.Phases.Delete, L("Permission:Phases.Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BillingResource>(name);
    }
}
