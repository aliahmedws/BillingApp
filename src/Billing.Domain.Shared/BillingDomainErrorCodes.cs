namespace Billing;

public static class BillingDomainErrorCodes
{
    public const string PhaseAlreadyExists = "Billing:PhaseAlreadyExists";
    public const string PhaseCodeAlreadyExists = "Billing:PhaseCodeAlreadyExists";
    public const string GovtChargeDecimalLimitExceeded = "Billing:GovtChargeDecimalLimitExceeded";
    public const string GovtChargeValueExceed = "Billing:GovtChargeExceedValue";
    public const string GovtChargeNegValue = "Billing:GovtChargeLessValue";
    //Iesco Charges
    public const string IescoChargeValueLimitExceeded = "Billing:IescoChargeValueLimitExceeded";
    public const string IescoChargeDecimalScale = "Billing:IescoChargeDecimalScale"; 
    public const string IescoChargeNegValue = "Billing:IescoChargeNegValue"; 

    public const string BlockNameAlreadyExist = "Billing:BlockNameAlreadyExist";
    public const string BlockCodeAlreadyExists = "Billing:BlockCodeAlreadyExists";
    public const string ConsumerCnicAlreadyExists = "Billing:ConsumerCnicAlreadyExists";
    public const string ConsumerPhoneAlreadyExists = "Billing:ConsumerPhoneAlreadyExists";
    //Society Charge
    public const string SocietyChargeValueLimitExceeded = "Billing:SocietyChargeValueLimitExceeded";
    public const string SocietyChargeNegValue = "Billing:SocietyChargeNegValueException";
    public const string SocietyChargeDecimalScale = "Billing:SocietyChargeDecimalScale";
    public const string SocietyChargeAlreadyExist = "Billing:SocietyChargeAlreadyExist";
    public const string DuplicateRecord = "FBRLink:00001";
    public const string DuplicateRecordWithPropertyName = "FBRLink:00002";
    public const string DuplicateRecordWithValue = "FBRLink:00003";
    public const string EmptyFile = "EmptyFileError:00004";
    public const string InvalidFileFormat = "InvalidFileFormatError:00005";
    public const string NullField = "NullField:00006";
    public const string PlotAlreadyExists = "Billing:PlotAlreadyExists";
    //plot size
    public const string PlotNegValue = "Billing:PlotNegValueException";

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
    public const string ElectricityBillAlreadyExists = "Billing:ElectricityBillAlreadyExists";
    public const string ExpireDateMustBeAfterIssueDate = "Billing:ExpireDateMustBeAfterIssueDate";
    public const string MaintenanceBillTotalBeforeDueDateCannotBeNegative = "Billing:MaintenanceBillTotalBeforeDueDateCannotBeNegative";
    public const string TarrifSlabValueLimit = "Billing.TarrifSlabValueLimit";
    public const string UnitPriceError = "Billing:UnitPriceError";
    public const string LowerSlabError = "Billing:LowerSlabError";
    public const string upperSlabError = "Billing:UpperSlabError";
    public const string LowerSlabAlreadyExists = "Billing:LowerSlabAlreadyExists";
    public const string UpperSlabAlreadyExists = "Billing:UpperSlabAlreadyExists";
    public const string UnitPriceAlreadyExists = "Billing:UnitPriceAlreadyExists";
    public const string UnitPriceLessError = "Billing:UnitPriceLessError";
    public const string DocumentEmpty = "Billing:DocumentEmpty";
    public const string PresentAndPreviousReading = "Billing:PresentAndPreviousReading";

    //Maintenance Payment History
    public const string MaintenancePaymentHistoryAlreadyExists = "Billing:MaintenancePaymentHistoryAlreadyExists";
    public const string PaymentReceivedNegative = "Billing:PaymentReceivedNegativeException";

}
