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

    // --- Alternative Info (all optional) ---
    public string? AlternativePersonName { get; set; }
    public string? AlternativePersonPhone { get; set; }
    public string? AlternativePersonEmail { get; set; }
    public string? AlternativePersonCNIC { get; set; }

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
        string? alternativePersonName = null,
        string? alternativePersonPhone = null,
        string? alternativePersonEmail = null,
        string? alternativePersonCNIC = null)
        : base(id)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetContact(phone, cnic, email);
        SetGender(gender);
        SetAlternativeContactPerson(alternativePersonName, alternativePersonPhone, alternativePersonEmail, alternativePersonCNIC);
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

    internal ConsumerPersonalInfo ChangeAlternativeContactPerson(string? name, string? phone, string? email, string? cnic)
    {
        SetAlternativeContactPerson(name, phone, email, cnic);
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

    private void SetAlternativeContactPerson(string? name, string? phone, string? email, string? cnic)
    {
        AlternativePersonName = name.IsNullOrWhiteSpace() ? null : Check.Length(name, nameof(AlternativePersonName), ConsumerPersonalInfoConsts.MaxAlternativePersonNameLength, 0);
        AlternativePersonPhone = phone.IsNullOrWhiteSpace() ? null : Check.Length(phone, nameof(AlternativePersonPhone), ConsumerPersonalInfoConsts.MaxAlternativePersonPhoneLength, 0);
        AlternativePersonEmail = email.IsNullOrWhiteSpace() ? null : Check.Length(email, nameof(AlternativePersonEmail), ConsumerPersonalInfoConsts.MaxAlternativePersonEmailLength, 0);
        AlternativePersonCNIC = cnic.IsNullOrWhiteSpace() ? null : Check.Length(cnic, nameof(AlternativePersonCNIC), ConsumerPersonalInfoConsts.MaxAlternativePersonCnicLength, 0);
    }

    private void SetGender(Gender gender)
    {
        Gender = gender;
    }
}

