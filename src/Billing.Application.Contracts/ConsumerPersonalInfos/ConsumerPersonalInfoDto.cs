using Billing.ConsumerDocuments;
using System;
using System.Collections.Generic;
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

    public string? AlternativePersonName { get; set; }
    public string? AlternativePersonPhone { get; set; }
    public string? AlternativePersonEmail { get; set; }
    public string? AlternativePersonCNIC { get; set; }

    public AddressDto Address { get; set; }

    public List<ConsumerDocumentDto> ConsumerDocuments { get; set; } = new();
}
