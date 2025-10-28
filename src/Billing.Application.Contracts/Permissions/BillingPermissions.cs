namespace Billing.Permissions;

public static class BillingPermissions
{
    public const string GroupName = "Billing";

    public static class Phases
    {
        public const string Default = GroupName + ".Phases";
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
    }
}
