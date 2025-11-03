namespace Billing;

public static class BillingDomainErrorCodes
{
    public const string PhaseAlreadyExists = "Billing:PhaseAlreadyExists";
    public const string PhaseCodeAlreadyExists = "Billing:PhaseCodeAlreadyExists";
    public const string BlockNameAlreadyExist = "Billing:BlockNameAlreadyExist";
    public const string BlockCodeAlreadyExists = "Billing:BlockCodeAlreadyExists";
    public const string ConsumerCnicAlreadyExists = "Billing:ConsumerCnicAlreadyExists";
    public const string ConsumerPhoneAlreadyExists = "Billing:ConsumerPhoneAlreadyExists";
    public const string DuplicateRecord = "FBRLink:00001";
    public const string DuplicateRecordWithPropertyName = "FBRLink:00002";
    public const string DuplicateRecordWithValue = "FBRLink:00003";
    public const string EmptyFile = "EmptyFileError:00004";
    public const string InvalidFileFormat = "InvalidFileFormatError:00005";
    public const string NullField = "NullField:00006";
    public const string PlotAlreadyExists = "Billing:PlotAlreadyExists";
    public const string ConsumerAlreadyHasPlot = "Billing:ConsumerAlreadyHasPlot";
    public const string MeterAlreadyExists = "Billing:MeterAlreadyExists";
}
