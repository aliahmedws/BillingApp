using System;
using Volo.Abp.Domain.Repositories;

namespace Billing.ConsumerDocuments;

public interface IConsumerDetailRepository : IRepository<ConsumerDocument, Guid>
{
}
