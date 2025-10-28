using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.ConsumerPersonalInfos;

public class UpdateConsumerPersonalInfoDto
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

    [StringLength(ConsumerPersonalInfoConsts.MaxGuardianNameLength)]
    public string? GuardianName { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxGuardianPhoneLength)]
    public string? GuardianPhone { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxGuardianEmailLength)]
    public string? GuardianEmail { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxGuardianCnicLength)]
    public string? GuardianCNIC { get; set; }

    [Required]
    public AddressDto Address { get; set; }
}
