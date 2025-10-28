using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.ConsumerPersonalInfos;

public class CreateConsumerPersonalInfoDto
{
    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxFirstNameLength)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxLastNameLength)]
    public string LastName { get; set; }

    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxPhoneLength)]
    public string Phone { get; set; }

    [Required]
    [StringLength(ConsumerPersonalInfoConsts.MaxCnicLength)]
    public string CNIC { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime DOB { get; set; }

    [StringLength(ConsumerPersonalInfoConsts.MaxEmailLength)]
    public string? Email { get; set; }

    // Guardian Info (optional)
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
