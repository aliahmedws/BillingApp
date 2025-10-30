using System;
using Volo.Abp.Domain.Repositories;

namespace Billing.ConsumerDocumentDetails;

public interface IConsumerDocumentDetailRepository : IRepository<ConsumerDocumentDetail, Guid>
{
}
