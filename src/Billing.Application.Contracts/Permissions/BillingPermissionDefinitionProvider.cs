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

        var blocksPermission = myGroup.AddPermission(BillingPermissions.Blocks.Default, L("Permission:Blocks"));
        blocksPermission.AddChild(BillingPermissions.Blocks.Create, L("Permission:Blocks.Create"));
        blocksPermission.AddChild(BillingPermissions.Blocks.Edit, L("Permission:Blocks.Edit"));
        blocksPermission.AddChild(BillingPermissions.Blocks.Delete, L("Permission:Blocks.Delete"));

        var plotSizesPermission = myGroup.AddPermission(BillingPermissions.PlotSizes.Default, L("Permission:PlotSizes"));
        plotSizesPermission.AddChild(BillingPermissions.PlotSizes.Create, L("Permission:PlotSizes.Create"));
        plotSizesPermission.AddChild(BillingPermissions.PlotSizes.Edit, L("Permission:PlotSizes.Edit"));
        plotSizesPermission.AddChild(BillingPermissions.PlotSizes.Delete, L("Permission:PlotSizes.Delete"));

        var consumerPersonalInfosPermission = myGroup.AddPermission(BillingPermissions.ConsumerPersonalInfos.Default, L("Permission:ConsumerPersonalInfos"));
        consumerPersonalInfosPermission.AddChild(BillingPermissions.ConsumerPersonalInfos.Create, L("Permission:ConsumerPersonalInfos.Create"));
        consumerPersonalInfosPermission.AddChild(BillingPermissions.ConsumerPersonalInfos.Edit, L("Permission:ConsumerPersonalInfos.Edit"));
        consumerPersonalInfosPermission.AddChild(BillingPermissions.ConsumerPersonalInfos.Delete, L("Permission:ConsumerPersonalInfos.Delete"));
        consumerPersonalInfosPermission.AddChild(BillingPermissions.ConsumerPersonalInfos.View, L("Permission:ConsumerPersonalInfos.View"));
        consumerPersonalInfosPermission.AddChild(BillingPermissions.ConsumerPersonalInfos.AttachDocument, L("Permission:ConsumerPersonalInfos.AttachDocument"));

        var plotInfosPermission = myGroup.AddPermission(BillingPermissions.PlotInfos.Default, L("Permission:PlotInfos"));
        plotInfosPermission.AddChild(BillingPermissions.PlotInfos.Create, L("Permission:PlotInfos.Create"));
        plotInfosPermission.AddChild(BillingPermissions.PlotInfos.Edit, L("Permission:PlotInfos.Edit"));
        plotInfosPermission.AddChild(BillingPermissions.PlotInfos.Delete, L("Permission:PlotInfos.Delete"));
        plotInfosPermission.AddChild(BillingPermissions.PlotInfos.View, L("Permission:PlotInfos.View"));
        plotInfosPermission.AddChild(BillingPermissions.PlotInfos.TransferPlot, L("Permission:PlotInfos.TransferPlot"));
        plotInfosPermission.AddChild(BillingPermissions.PlotInfos.AttachDocument, L("Permission:PlotInfos.AttachDocument"));

        var meterInfosPermission = myGroup.AddPermission(BillingPermissions.MeterInfos.Default, L("Permission:MeterInfos"));
        meterInfosPermission.AddChild(BillingPermissions.MeterInfos.Create, L("Permission:MeterInfos.Create"));
        meterInfosPermission.AddChild(BillingPermissions.MeterInfos.Edit, L("Permission:MeterInfos.Edit"));
        meterInfosPermission.AddChild(BillingPermissions.MeterInfos.Delete, L("Permission:MeterInfos.Delete"));
        meterInfosPermission.AddChild(BillingPermissions.MeterInfos.View, L("Permission:MeterInfos.View"));
        meterInfosPermission.AddChild(BillingPermissions.MeterInfos.AttachDocument, L("Permission:MeterInfos.AttachDocument"));

        var plotTransferHistoriesPermission = myGroup.AddPermission(BillingPermissions.PlotTransferHistories.Default, L("Permission:PlotTransferHistories"));
        plotTransferHistoriesPermission.AddChild(BillingPermissions.PlotTransferHistories.Create, L("Permission:PlotTransferHistories.Create"));
        plotTransferHistoriesPermission.AddChild(BillingPermissions.PlotTransferHistories.Edit, L("Permission:PlotTransferHistories.Edit"));
        plotTransferHistoriesPermission.AddChild(BillingPermissions.PlotTransferHistories.Delete, L("Permission:PlotTransferHistories.Delete"));
        plotTransferHistoriesPermission.AddChild(BillingPermissions.PlotTransferHistories.View, L("Permission:PlotTransferHistories.View"));
        plotTransferHistoriesPermission.AddChild(BillingPermissions.PlotTransferHistories.Approved, L("Permission:PlotTransferHistories.Approved"));
        plotTransferHistoriesPermission.AddChild(BillingPermissions.PlotTransferHistories.Reject, L("Permission:PlotTransferHistories.Reject"));

        var tarrifSlabPermission = myGroup.AddPermission(BillingPermissions.TarrifSlabs.Default, L("Permission:TarrifSlabs"));
        tarrifSlabPermission.AddChild(BillingPermissions.TarrifSlabs.Edit, L("Permission:TarrifSlabs.Edit"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BillingResource>(name);
    }
}
