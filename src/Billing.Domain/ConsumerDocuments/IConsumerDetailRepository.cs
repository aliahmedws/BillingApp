using Billing.ConsumerDocumentDetails;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.ConsumerDocuments;

public interface IConsumerDetailRepository : IRepository<ConsumerDocument, Guid>
{
    Task<List<ConsumerDocument>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        Guid? consumerId,
        DocumentType? documentType,
        bool? isVerfied,
        DateTime? issueDate,
        DateTime? expireDate);

    Task<long> GetCountAsync(
        string? filter,
        Guid? consumerId,
        DocumentType? documentType,
        bool? isVerfied,
        DateTime? issueDate,
        DateTime? expireDate);
}
