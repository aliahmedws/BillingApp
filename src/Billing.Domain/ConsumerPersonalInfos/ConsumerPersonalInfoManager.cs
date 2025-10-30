using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.ConsumerPersonalInfos;

public class ConsumerPersonalInfoManager : DomainService
{
    private readonly IConsumerPersonalInfoRepository _consumerRepository;

    public ConsumerPersonalInfoManager(IConsumerPersonalInfoRepository consumerRepository)
    {
        _consumerRepository = consumerRepository;
    }

    public async Task<ConsumerPersonalInfo> CreateAsync(
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
    {
        Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
        Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
        Check.NotNullOrWhiteSpace(phone, nameof(phone));
        Check.NotNullOrWhiteSpace(cnic, nameof(cnic));
        Check.NotNull(address, nameof(address));

        var existingByCnic = await _consumerRepository.FindByCnicAsync(cnic);
        if (existingByCnic != null)
        {
            throw new ConsumerCnicAlreadyExistsException(cnic);
        }

        // 🔍 Check for duplicate Phone
        var existingByPhone = await _consumerRepository.FindByPhoneAsync(phone);
        if (existingByPhone != null)
        {
            throw new ConsumerPhoneAlreadyExistsException(phone);
        }

        return new ConsumerPersonalInfo(
            GuidGenerator.Create(),
            firstName,
            lastName,
            phone,
            cnic,
            gender,
            dob,
            address,
            email,
            guardianName,
            guardianPhone,
            guardianEmail,
            guardianCNIC
        );
    }

    public async Task UpdateAsync(
        ConsumerPersonalInfo consumer,
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
    {
        Check.NotNull(consumer, nameof(consumer));
        Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
        Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
        Check.NotNullOrWhiteSpace(phone, nameof(phone));
        Check.NotNullOrWhiteSpace(cnic, nameof(cnic));
        Check.NotNull(address, nameof(address));

        var existingByCnic = await _consumerRepository.FindByCnicAsync(cnic);
        if (existingByCnic != null && existingByCnic.Id != consumer.Id)
        {
            throw new ConsumerCnicAlreadyExistsException(cnic);
        }

        var existingByPhone = await _consumerRepository.FindByPhoneAsync(phone);
        if (existingByPhone != null && existingByPhone.Id != consumer.Id)
        {
            throw new ConsumerPhoneAlreadyExistsException(phone);
        }

        consumer
            .ChangeFirstName(firstName)
            .ChangeLastName(lastName)
            .ChangeContact(phone, cnic, email)
            .ChangeGender(gender)
            .ChangeDOB(dob)
            .ChangeGuardian(guardianName, guardianPhone, guardianEmail, guardianCNIC)
            .ChangeAddress(address);
    }
}
