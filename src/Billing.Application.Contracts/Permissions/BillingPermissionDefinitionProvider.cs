using Billing.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

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

        var govtChargesPermission = myGroup.AddPermission(BillingPermissions.GovtCharges.Default, L("Permission:GovtCharges"));
        govtChargesPermission.AddChild(BillingPermissions.GovtCharges.Edit, L("Permission:GovtCharges.Edit"));

        var iescoChargesPermission = myGroup.AddPermission(BillingPermissions.IescoCharges.Default, L("Permission:IescoCharges"));
        iescoChargesPermission.AddChild(BillingPermissions.IescoCharges.Edit, L("Permission:IescoCharges.Edit"));

        var societyChargesPermission = myGroup.AddPermission(BillingPermissions.SocietyCharges.Default, L("Permission:SocietyCharges"));
        societyChargesPermission.AddChild(BillingPermissions.SocietyCharges.Create, L("Permission:SocietyCharges.Create"));
        societyChargesPermission.AddChild(BillingPermissions.SocietyCharges.Edit, L("Permission:SocietyCharges.Edit"));
        societyChargesPermission.AddChild(BillingPermissions.SocietyCharges.Delete, L("Permission:SocietyCharges.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BillingResource>(name);
    }
}
