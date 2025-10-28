using Volo.Abp;

namespace Billing.Blocks;

public class BlockCodeAlreadyExistException : BusinessException
{
    public BlockCodeAlreadyExistException(string blockCode) : base(BillingDomainErrorCodes.BlockCodeAlreadyExists)
    {
        WithData("blockCode", blockCode);
    }
}
