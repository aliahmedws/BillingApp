using System;
using Volo.Abp.Domain.Repositories;

namespace Billing.MeterDocuments;

public interface IMeterDocumentRepository : IRepository<MeterDocument, Guid>
{
}
