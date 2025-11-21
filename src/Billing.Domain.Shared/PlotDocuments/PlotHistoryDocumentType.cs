namespace Billing.PlotDocuments;

public enum PlotHistoryDocumentType
{

    TransferDeed = 1,           // Sale / transfer deed
    RegistryCopy = 2,           // Registered document from registrar

    CnicCopy = 3,               // CNIC / ID copy (buyer or seller)
    NoObjectionCertificate = 4, // NOC from society / authority

    PaymentReceipt = 5,         // Token / full payment receipt
    PowerOfAttorney = 6,        // POA used for this transfer
    MutationOrder = 7,          // Intiqal / mutation order

    Other = 8                   // Any additional supporting doc
}
