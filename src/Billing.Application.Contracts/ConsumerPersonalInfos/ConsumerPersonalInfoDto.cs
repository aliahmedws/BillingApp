using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerPersonalInfos;

public class ConsumerPersonalInfoDto : EntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string CNIC { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DOB { get; set; }
    public string? Email { get; set; }

    public string? GuardianName { get; set; }
    public string? GuardianPhone { get; set; }
    public string? GuardianEmail { get; set; }
    public string? GuardianCNIC { get; set; }

    public AddressDto Address { get; set; }
}
