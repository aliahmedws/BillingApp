using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.ConsumerPersonalInfos;

public class ConsumerPersonalInfo : FullAuditedAggregateRoot<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string CNIC { get; set; }
    public Gender Gender { get; set; }
    public DateTime DOB { get; set; }
    public string? Email { get; set; }

    // --- Guardian Info (all optional) ---
    public string? GuardianName { get; set; }
    public string? GuardianPhone { get; set; }
    public string? GuardianEmail { get; set; }
    public string? GuardianCNIC { get; set; }

    // --- Value Object ---
    public Address Address { get; set; }

    private ConsumerPersonalInfo() { }

    internal ConsumerPersonalInfo(
        Guid id,
        string firstName,
        string lastName,
        string phone,
        string cnic,
        Gender gender,
        DateTime dob,
        Address address,
        string? email = null,
        string? guardianName = null,
        string? guardianPhone = null,
        string? guardianEmail = null,
        string? guardianCNIC = null)
        : base(id)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetContact(phone, cnic, email);
        SetGender(gender);
        SetGuardian(guardianName, guardianPhone, guardianEmail, guardianCNIC);
        Address = Check.NotNull(address, nameof(address));
        DOB = dob;
    }
    internal ConsumerPersonalInfo ChangeFirstName(string firstName)
    {
        SetFirstName(firstName);
        return this;
    }

    internal ConsumerPersonalInfo ChangeLastName(string lastName)
    {
        SetLastName(lastName);
        return this;
    }

    internal ConsumerPersonalInfo ChangeGuardian(string? name, string? phone, string? email, string? cnic)
    {
        SetGuardian(name, phone, email, cnic);
        return this;
    }

    internal ConsumerPersonalInfo ChangeContact(string phone, string cnic, string? email)
    {
        SetContact(phone, cnic, email);
        return this;
    }

    internal ConsumerPersonalInfo ChangeGender(Gender gender)
    {
        SetGender(gender);
        return this;
    }

    internal ConsumerPersonalInfo ChangeAddress(Address address)
    {
        Address = Check.NotNull(address, nameof(address));
        return this;
    }

    internal ConsumerPersonalInfo ChangeDOB(DateTime dob)
    {
        DOB = dob;
        return this;
    }

    private void SetFirstName(string firstName)
    {
        FirstName = Check.NotNullOrWhiteSpace(firstName, nameof(FirstName), maxLength: ConsumerPersonalInfoConsts.MaxFirstNameLength);
    }

    private void SetLastName(string lastName)
    {
        LastName = Check.NotNullOrWhiteSpace(lastName, nameof(LastName), maxLength: ConsumerPersonalInfoConsts.MaxLastNameLength);
    }

    private void SetContact(string phone, string cnic, string? email)
    {
        Phone = Check.NotNullOrWhiteSpace(phone, nameof(Phone), maxLength: ConsumerPersonalInfoConsts.MaxPhoneLength);
        CNIC = Check.NotNullOrWhiteSpace(cnic, nameof(CNIC), maxLength: ConsumerPersonalInfoConsts.MaxCnicLength);
        if (!email.IsNullOrWhiteSpace())
        {
            Email = Check.Length(email, nameof(Email), ConsumerPersonalInfoConsts.MaxEmailLength, 0);
        }
        else
        {
            Email = null;
        }
    }

    private void SetGuardian(string? name, string? phone, string? email, string? cnic)
    {
        GuardianName = name.IsNullOrWhiteSpace() ? null : Check.Length(name, nameof(GuardianName), ConsumerPersonalInfoConsts.MaxGuardianNameLength, 0);
        GuardianPhone = phone.IsNullOrWhiteSpace() ? null : Check.Length(phone, nameof(GuardianPhone), ConsumerPersonalInfoConsts.MaxGuardianPhoneLength, 0);
        GuardianEmail = email.IsNullOrWhiteSpace() ? null : Check.Length(email, nameof(GuardianEmail), ConsumerPersonalInfoConsts.MaxGuardianEmailLength, 0);
        GuardianCNIC = cnic.IsNullOrWhiteSpace() ? null : Check.Length(cnic, nameof(GuardianCNIC), ConsumerPersonalInfoConsts.MaxGuardianCnicLength, 0);
    }

    private void SetGender(Gender gender)
    {
        Gender = gender;
    }
}

