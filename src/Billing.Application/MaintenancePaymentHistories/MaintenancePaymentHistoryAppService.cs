using Billing.Localization;
using Billing.MaintenanceBills;
using Billing.Permissions;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Billing.MaintenancePaymentHistories;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.MaintenancePaymentHistories.Default)]
public class MaintenancePaymentHistoryAppService : BillingAppService, IMaintenancePaymentHistoryAppService
{
    private readonly IMaintenancePaymentHistoryRepository _maintenancePaymentHistoryRepository;
    private readonly MaintenancePaymentHistoryManager _maintenancePaymentHistoryManager;
    private readonly IMaintenanceBillRepository _maintenanceBillRepository;
    private readonly IStringLocalizer<BillingResource> _localizer;

    public MaintenancePaymentHistoryAppService(
        IMaintenancePaymentHistoryRepository maintenancePaymentHistoryRepository,
        MaintenancePaymentHistoryManager maintenancePaymentHistoryManager,
        IMaintenanceBillRepository maintenanceBillRepository,
        IStringLocalizer<BillingResource> localizer)
    {
        _maintenancePaymentHistoryRepository = maintenancePaymentHistoryRepository;
        _maintenancePaymentHistoryManager = maintenancePaymentHistoryManager;
        _maintenanceBillRepository = maintenanceBillRepository;
        _localizer = localizer;
    }

    public async Task<MaintenancePaymentHistoryDto> GetAsync(Guid id)
    {
        var paymentHistory = await _maintenancePaymentHistoryRepository.GetAsync(id);
        return ObjectMapper.Map<MaintenancePaymentHistory, MaintenancePaymentHistoryDto>(paymentHistory);
    }

    public async Task<PagedResultDto<MaintenancePaymentHistoryDto>> GetListAsync(GetMaintenancePaymentHistoryListDto input)
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

    [Authorize(BillingPermissions.MaintenancePaymentHistories.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _maintenancePaymentHistoryRepository.DeleteAsync(id);
    }

    public async Task<IRemoteStreamContent> DownloadImportTemplateAsync()
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("Template");

        sheet.Cell(1, 1).Value = _localizer["MaintenanceBillId"].Value;
        sheet.Cell(1, 2).Value = _localizer["TransactionId"].Value;
        sheet.Cell(1, 3).Value = _localizer["PaymentReceived"].Value;
        sheet.Cell(1, 4).Value = _localizer["PaymentDate"].Value;
        sheet.Cell(1, 5).Value = _localizer["Method"].Value;

        var stream = new MemoryStream();
        workbook.SaveAs(stream);

        stream.Seek(0, SeekOrigin.Begin);


        return new RemoteStreamContent(
            stream,
            "PaymentImportTemplate.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    public async Task<ImportResultDto> ImportExcelFileAsync(IFormFile file)
    {
        var result = new ImportResultDto();
        var rows = new List<MaintenancePaymentExcelDto>();

        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            ms.Position = 0;

            using var workbook = new XLWorkbook(ms);
            var sheet = workbook.Worksheet(1);

            int lastRow = sheet.LastRowUsed().RowNumber();


            for (int row = 2; row <= lastRow; row++)
            {
                var billIdString = sheet.Cell(row, 1).GetString().Trim();
                
                if(!Guid.TryParse(billIdString, out var billId))
                {
                    result.Errors.Add($"{_localizer["Row"]} {row}: {_localizer["InvalidMaintenanceBillId"]} ({billIdString})");
                    continue;
                }

                try
                {
                    var dto = new MaintenancePaymentExcelDto
                    {
                        MaintenanceBillId = billId,
                        TransactionId = sheet.Cell(row, 2).GetValue<string>()?.Trim(),
                        PaymentReceived = sheet.Cell(row, 3).GetValue<decimal>(),
                        PaymentDate = sheet.Cell(row, 4).GetValue<DateTime>(),
                        Method = sheet.Cell(row, 5).GetValue<string>()?.Trim()
                    };

                    rows.Add(dto);
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"{_localizer["Row"]} {row}: {_localizer["InvalidDataFormat"]} ({ex.Message})");
                }
            }
        }

        if (rows.Count == 0)
            throw new UserFriendlyException(_localizer["ExcelNoDataFound"]);

        foreach (var row in rows)
        {
            try
            {
                if (row.MaintenanceBillId == Guid.Empty)
                    throw new UserFriendlyException(_localizer["MaintenanceBillIdRequired"]);

                if (string.IsNullOrWhiteSpace(row.TransactionId))
                    throw new UserFriendlyException(_localizer["TransactionIdRequired"]);

                if (row.PaymentReceived <= 0)
                    throw new UserFriendlyException(_localizer["PaymentReceivedGreaterThanZero"]);

                if (string.IsNullOrWhiteSpace(row.Method))
                    throw new UserFriendlyException(_localizer["PaymentMethodRequired"]);

                var bill = await _maintenanceBillRepository.FindAsync(row.MaintenanceBillId);
                if (bill == null)
                {
                    result.Errors.Add($"{_localizer["BillNotFound"]}: {row.MaintenanceBillId}");
                    continue;
                }

                if (!Enum.TryParse<PaymentMethod>(row.Method, true, out var paymentMethod))
                {
                    result.Errors.Add($"{_localizer["InvalidPaymentMethod"]}: {row.Method}");
                    continue;
                }

                var history = await _maintenancePaymentHistoryManager.CreateAsync(
                    row.MaintenanceBillId,
                    row.TransactionId,
                    row.PaymentReceived,
                    row.PaymentDate,
                    paymentMethod
                );

                await _maintenancePaymentHistoryRepository.InsertAsync(history);

                result.SuccessCount++;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"{_localizer["ErrorProcessingRow"]} {row.MaintenanceBillId}: {ex.Message}");
            }
        }

        return result;
    }

}
