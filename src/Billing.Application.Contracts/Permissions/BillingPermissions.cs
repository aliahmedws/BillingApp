namespace Billing.Permissions;

public static class BillingPermissions
{
    public const string GroupName = "Billing";

    public static class MainHeading
    {
        public const string SocietySetup = GroupName + ".MainHeading.SocietySetup";
        public const string Consumer = GroupName + ".MainHeading.Consumer";
        public const string PlotTypes = GroupName + ".MainHeading.PlotTypes";
    }

    public static class Phases
    {
        public const string Default = GroupName + ".Phases";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class GovtCharges
    {
        public const string Default = GroupName + ".GovtCharges";
        public const string Edit = Default + ".Edit";
    }

    public static class IescoCharges
    {
        public const string Default = GroupName + ".IescoCharges";
        public const string Edit = Default + ".Edit";
    }

    public static class SocietyCharges
    {
        public const string Default = GroupName + ".SocietyCharges";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class Blocks
        {
            public const string Default = GroupName + ".Blocks";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class PlotSizes
        {
            public const string Default = GroupName + ".PlotSizes";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

    public static class ConsumerPersonalInfos
    {
        public const string Default = GroupName + ".ConsumerPersonalInfos";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string AttachDocument = Default + ".AttachDocument";
    }
    
    public static class PlotInfos
    {
        public const string Default = GroupName + ".PlotInfos";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string TransferPlot = Default + ".TransferPlot";
        public const string AttachDocument = Default + ".AttachDocument";
    }
    
    public static class MeterInfos
    {
        public const string Default = GroupName + ".MeterInfos";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string AttachDocument = Default + ".AttachDocument";
    }
    
    public static class PlotTransferHistories
    {
        public const string Default = GroupName + ".PlotTransferHistories";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string Approved = Default + ".Approved";
        public const string Reject = Default + ".Reject";
        public const string AttachDocument = Default + ".AttachDocument";
    }
    public static class TarrifSlabs
    {
        public const string Default = GroupName + ".TarrifSlabs";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
      //public const string View = Default + ".View";
    }

    public static class MaintenanceBills
    {
        public const string Default = GroupName + ".MaintenanceBills";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }


}
