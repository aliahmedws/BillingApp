using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.MaintenancePaymentHistories;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.MaintenancePaymentHistories.Default)]
public class MaintenancePaymentHistoryAppService : BillingAppService, IMaintenancePaymentHistoryAppService
{
    private readonly IMaintenancePaymentHistoryRepository _maintenancePaymentHistoryRepository;
    private readonly MaintenancePaymentHistoryManager _maintenancePaymentHistoryManager;

    public MaintenancePaymentHistoryAppService(
        IMaintenancePaymentHistoryRepository maintenancePaymentHistoryRepository,
        MaintenancePaymentHistoryManager maintenancePaymentHistoryManager)
    {
        _maintenancePaymentHistoryRepository = maintenancePaymentHistoryRepository;
        _maintenancePaymentHistoryManager = maintenancePaymentHistoryManager;
    }

    //GET ASYNC
    public async Task<MaintenancePaymentHistoryDto> GetAsync(Guid id)
    {
        var paymentHistory = await _maintenancePaymentHistoryRepository.GetAsync(id);
        return ObjectMapper.Map<MaintenancePaymentHistory, MaintenancePaymentHistoryDto>(paymentHistory);
    }

    //GET LIST ASYNC
    public async Task<PagedResultDto<MaintenancePaymentHistoryDto>> GetListAsync(
     GetMaintenancePaymentHistoryListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(MaintenancePaymentHistory.CreationTime);
        }

        var paymentHistories = await _maintenancePaymentHistoryRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.MaintenanceBillId,
            input.Filter
        );

        var totalCount = await _maintenancePaymentHistoryRepository.GetCountAsync(
            input.Filter,
            input.Method,
            input.PaymentDate,
            input.MaintenanceBillId
        );

        return new PagedResultDto<MaintenancePaymentHistoryDto>(
        totalCount,
        ObjectMapper.Map<List<MaintenancePaymentHistory>, List<MaintenancePaymentHistoryDto>>(paymentHistories));

    }
    
    //CREATE ASYNC
    [Authorize(BillingPermissions.MaintenancePaymentHistories.Create)]
    public async Task<MaintenancePaymentHistoryDto> CreateAsync(CreateMaintenancePaymentHistoryDto input)
    {
        var paymentHistory = await _maintenancePaymentHistoryManager.CreateAsync(
            input.MaintenanceBillId,
            input.TransactionId,
            input.PaymentReceived,
            input.PaymentDate,
            input.Method
        );

        await _maintenancePaymentHistoryRepository.InsertAsync(paymentHistory);
        return ObjectMapper.Map<MaintenancePaymentHistory, MaintenancePaymentHistoryDto>(paymentHistory);
    }

    //UPDATE ASYNC
    [Authorize(BillingPermissions.MaintenancePaymentHistories.Edit)]
    public async Task UpdateAsync(Guid id, UpdateMaintenancePaymentHistoryDto input)
    {
        var paymentHistory = await _maintenancePaymentHistoryRepository.GetAsync(id);

        await _maintenancePaymentHistoryManager.UpdateAsync(
            paymentHistory,
            input.TransactionId,
            input.PaymentReceived,
            input.PaymentDate,
            input.Method
            );

        await _maintenancePaymentHistoryRepository.UpdateAsync(paymentHistory);
    }

    //DELETE
    [Authorize(BillingPermissions.MaintenancePaymentHistories.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _maintenancePaymentHistoryRepository.DeleteAsync(id);
    }

}
