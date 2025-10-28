using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerPersonalInfos;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.ConsumerPersonalInfos.Default)]
public class ConsumerPersonalInfoAppService : BillingAppService, IConsumerPersonalInfoAppService
{
    private readonly IConsumerPersonalInfoRepository _consumerRepository;
    private readonly ConsumerPersonalInfoManager _consumerManager;

    public ConsumerPersonalInfoAppService(
        IConsumerPersonalInfoRepository consumerRepository,
        ConsumerPersonalInfoManager consumerManager)
    {
        _consumerRepository = consumerRepository;
        _consumerManager = consumerManager;
    }

    [Authorize(BillingPermissions.ConsumerPersonalInfos.Create)]
    public async Task<ConsumerPersonalInfoDto> CreateAsync(CreateConsumerPersonalInfoDto input)
    {
        var address = new Address(
            input.Address.Street,
            input.Address.City,
            input.Address.State,
            input.Address.Country,
            input.Address.PostalCode
        );

        var consumer = await _consumerManager.CreateAsync(
            input.FirstName,
            input.LastName,
            input.Phone,
            input.CNIC,
            input.Gender,
            input.DOB,
            address,
            input.Email,
            input.GuardianName,
            input.GuardianPhone,
            input.GuardianEmail,
            input.GuardianCNIC
        );

        await _consumerRepository.InsertAsync(consumer);
        return ObjectMapper.Map<ConsumerPersonalInfo, ConsumerPersonalInfoDto>(consumer);
    }

    [Authorize(BillingPermissions.ConsumerPersonalInfos.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _consumerRepository.DeleteAsync(id);
    }

    public async Task<ConsumerPersonalInfoDto> GetAsync(Guid id)
    {
        var consumer = await _consumerRepository.GetAsync(id);
        return ObjectMapper.Map<ConsumerPersonalInfo, ConsumerPersonalInfoDto>(consumer);
    }

    public async Task<PagedResultDto<ConsumerPersonalInfoDto>> GetListAsync(GetConsumerPersonalInfoListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(ConsumerPersonalInfo.CreationTime);
        }

        var consumers = await _consumerRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.FirstName,
            input.LastName,
            input.CNIC,
            input.Gender
        );

        var totalCount = await _consumerRepository.GetCountAsync(
            input.Filter,
            input.FirstName,
            input.LastName,
            input.CNIC,
            input.Gender
        );

        return new PagedResultDto<ConsumerPersonalInfoDto>(
            totalCount,
            ObjectMapper.Map<List<ConsumerPersonalInfo>, List<ConsumerPersonalInfoDto>>(consumers)
        );
    }

    [Authorize(BillingPermissions.ConsumerPersonalInfos.Edit)]
    public async Task UpdateAsync(Guid id, UpdateConsumerPersonalInfoDto input)
    {
        var consumer = await _consumerRepository.GetAsync(id);

        var address = new Address(
            input.Address.Street,
            input.Address.City,
            input.Address.State,
            input.Address.Country,
            input.Address.PostalCode
        );

        await _consumerManager.UpdateAsync(
            consumer,
            input.FirstName,
            input.LastName,
            input.Phone,
            input.CNIC,
            input.Gender,
            input.DOB,
            address,
            input.Email,
            input.GuardianName,
            input.GuardianPhone,
            input.GuardianEmail,
            input.GuardianCNIC
        );

        await _consumerRepository.UpdateAsync(consumer);
    }
}
