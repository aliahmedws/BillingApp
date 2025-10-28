using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerPersonalInfos;

public class GetConsumerPersonalInfoListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CNIC { get; set; }
    public Gender? Gender { get; set; }
}
