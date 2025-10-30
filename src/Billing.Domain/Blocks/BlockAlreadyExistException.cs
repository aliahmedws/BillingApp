using Volo.Abp;

namespace Billing.Blocks;

public class BlockAlreadyExistException : BusinessException
{
    public BlockAlreadyExistException(string blockName) : base(BillingDomainErrorCodes.BlockNameAlreadyExist)
    {
        WithData("blockName", blockName);
    }
}
