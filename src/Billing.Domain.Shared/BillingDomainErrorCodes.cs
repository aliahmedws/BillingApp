namespace Billing;

public static class BillingDomainErrorCodes
{
    public const string PhaseAlreadyExists = "Billing:PhaseAlreadyExists";
    public const string PhaseCodeAlreadyExists = "Billing:PhaseCodeAlreadyExists";
    public const string GovtChargeValueLimitExceeded = "Billing:GovtChargeValueLimitExceeded";
    public const string IescoChargeValueLimitExceeded = "Billing:IescoChargeValueLimitExceeded";
    public const string SocietyChargeValueLimitExceeded = "Billing:SocietyChargeValueLimitExceeded";
    public const string BlockNameAlreadyExist = "Billing:BlockNameAlreadyExist";
    public const string BlockCodeAlreadyExists = "Billing:BlockCodeAlreadyExists";
    public const string ConsumerCnicAlreadyExists = "Billing:ConsumerCnicAlreadyExists";
    public const string ConsumerPhoneAlreadyExists = "Billing:ConsumerPhoneAlreadyExists";
    public const string SocietyChargeAlreadyExist = "Billing:SocietyChargeAlreadyExist";
    public const string SocietyChargeValueLimit = "Billing.SocietyChargeValueLimit";
    public const string DuplicateRecord = "FBRLink:00001";
    public const string DuplicateRecordWithPropertyName = "FBRLink:00002";
    public const string DuplicateRecordWithValue = "FBRLink:00003";
    public const string EmptyFile = "EmptyFileError:00004";
    public const string InvalidFileFormat = "InvalidFileFormatError:00005";
    public const string NullField = "NullField:00006";
    public const string PlotAlreadyExists = "Billing:PlotAlreadyExists";
    public const string ConsumerAlreadyHasPlot = "Billing:ConsumerAlreadyHasPlot";
    public const string MeterAlreadyExists = "Billing:MeterAlreadyExists";
    public const string InvalidConsumerExists = "Billing:InvalidConsumerExists";
    public const string PlotTransferRegistryAlreadyExists = "Billing:PlotTransferRegistryAlreadyExists";
    public const string PlotTransferAlreadyApproved = "Billing:PlotTransferAlreadyApproved";
    public const string PlotTransferAlreadyRejected = "Billing:PlotTransferAlreadyRejected";
    public const string BillingCalculationError = "Billing:ValueCantBeNegative";
    public const string BillingDateError = "Billing:IssueDateDueDate";
    public const string BillingNegativeValueError = "Billing:BillingNegativeValueError";
    public const string NegativeAmountNotAllowed = "Billing:NegativeAmountNotAllowed";
    public const string MaintenanceBillAlreadyExists = "Billing:MaintenanceBillAlreadyExists";
    public const string ExpireDateMustBeAfterIssueDate = "Billing:ExpireDateMustBeAfterIssueDate";
    public const string MaintenanceBillTotalBeforeDueDateCannotBeNegative = "Billing:MaintenanceBillTotalBeforeDueDateCannotBeNegative";
    //Tarrif Slab
    public const string TarrifSlabValueLimit = "Billing.TarrifSlabValueLimit";
    public const string UnitPriceError = "Billing:UnitPriceError";
    public const string LowerSlabError = "Billing:LowerSlabError";
    public const string upperSlabError = "Billing:UpperSlabError";
    public const string DocumentEmpty = "Billing:DocumentEmpty";

}
