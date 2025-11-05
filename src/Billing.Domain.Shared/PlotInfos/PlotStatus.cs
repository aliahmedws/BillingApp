namespace Billing.PlotInfos;

public enum PlotStatus
{
    Available = 1,          // Ready for sale or allotment
    Reserved = 2,           // Temporarily blocked or reserved
    Sold = 3,               // Fully sold and transferred
    UnderProcess = 4,       // Documentation or verification ongoing
    Cancelled = 5,          // Booking cancelled
    UnderConstruction = 6,  // Development or building in progress
    Completed = 7,          // Construction finished and possession ready
    Leased = 8,             // On lease/rental basis
    Mortgaged = 9,          // Pledged to a bank/authority
    Disputed = 10,          // Legal or ownership dispute
    Transferred = 11,       // Ownership transferred to new buyer
    Possessed = 12,         // Buyer has taken physical possession
    Repossessed = 13,       // Taken back by developer/authority
    Inactive = 14,          // Temporarily inactive/unlisted
    UnderReview = 15        // Awaiting admin approval or validation
}
