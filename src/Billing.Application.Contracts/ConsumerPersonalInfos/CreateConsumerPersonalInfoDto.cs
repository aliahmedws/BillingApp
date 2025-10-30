using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.ConsumerPersonalInfos;

public class CreateConsumerPersonalInfoDto
{
    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxFirstNameLength)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxLastNameLength)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxPhoneLength)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxCnicLength)]
    public string CNIC { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime DOB { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxEmailLength)]
    public string? Email { get; set; }

    // Guardian Info (optional)
    [StringLength(ConsumerPersonalInfoConsts.MaxAlternativePersonNameLength)]
    public string? AlternativePersonName { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxAlternativePersonPhoneLength)]
    public string? AlternativePersonPhone { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxAlternativePersonEmailLength)]
    public string? AlternativePersonEmail { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxAlternativePersonCnicLength)]
    public string? AlternativePersonCNIC { get; set; }

    [Required]
    public AddressDto Address { get; set; }
}
