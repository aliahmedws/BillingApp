using Billing.ElectricityBills;
using Billing.Localization;
using Billing.MaintenancePaymentHistories;
using Billing.Permissions;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Billing.ElectricityPaymentHistories;

[RemoteService(false)]
[Authorize(BillingPermissions.ElectricityPaymentHistories.Default)]
public class ElectricityPaymentHistoryAppService : ApplicationService, IElectricityPaymentHistoryAppService
{
    private readonly IElectricityPaymentHistoryRepository _electricityPaymentHistoryRepository;
    private readonly ElectricityPaymentHistoryManager _electricityPaymentHistoryManager;
    private readonly IStringLocalizer<BillingResource> _localizer;
    private readonly IElectricityBillRepository _electricityBillRepository;

    public ElectricityPaymentHistoryAppService(
        IElectricityPaymentHistoryRepository electricityPaymentHistoryRepository,
        ElectricityPaymentHistoryManager electricityPaymentHistoryManager,
        IStringLocalizer<BillingResource> localizer,
        IElectricityBillRepository electricityBillRepository)
    {
        _electricityPaymentHistoryRepository = electricityPaymentHistoryRepository;
        _electricityPaymentHistoryManager = electricityPaymentHistoryManager;
        _localizer = localizer;
        _electricityBillRepository = electricityBillRepository;
    }

    public async Task<ElectricityPaymentHistoryDto> GetAsync(Guid id)
    {
        var paymentHistory = await _electricityPaymentHistoryRepository.GetAsync(id);
        return ObjectMapper.Map<ElectricityPaymentHistory, ElectricityPaymentHistoryDto>(paymentHistory);
    }

    public async Task<PagedResultDto<ElectricityPaymentHistoryDto>> GetListAsync(GetElectricityPaymentHistoryListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(ElectricityPaymentHistory.CreationTime);
        }

        var totalCount = await _electricityPaymentHistoryRepository.GetCountAsync(
          input.Filter,
          input.TransactionId,
          input.Method,
          input.ElectricityBillId
          );

        var items = await _electricityPaymentHistoryRepository.GetListAsync(
           input.SkipCount,
           input.MaxResultCount,
           input.Sorting,
           input.Filter,
           input.TransactionId,
           input.Method,
           input.ElectricityBillId
           );

        var itemsDto = ObjectMapper.Map<List<ElectricityPaymentHistory>, List<ElectricityPaymentHistoryDto>>(items);

        return new PagedResultDto<ElectricityPaymentHistoryDto>(
            totalCount,
            itemsDto
        );
    }

    [Authorize(BillingPermissions.ElectricityPaymentHistories.Create)]
    public async Task<ElectricityPaymentHistoryDto> CreateAsync(CreateElectricityPaymentHistoryDto input)
    {
        var existingPayment = await _electricityPaymentHistoryRepository.FindByTransactionIdAsync(input.TransactionId);
        if (existingPayment != null)
        {
            throw new ElectricityPaymentAlreadyExistsException(input.TransactionId);
        }

        var paymentHistory = await _electricityPaymentHistoryManager.CreateAsync(
            input.ElectricityBillId,
            input.TransactionId,
            input.PaymentReceived,
            input.PaymentDate,
            input.Method
        );

        await _electricityPaymentHistoryRepository.InsertAsync(paymentHistory);
        return ObjectMapper.Map<ElectricityPaymentHistory, ElectricityPaymentHistoryDto>(paymentHistory);
    }

    [Authorize(BillingPermissions.ElectricityPaymentHistories.Edit)]
    public async Task UpdateAsync(Guid id, UpdateElectricityPaymentHistoryDto input)
    {
        var paymentHistory = await _electricityPaymentHistoryRepository.GetAsync(id);

        await _electricityPaymentHistoryManager.UpdateElectricityPaymentHistoryAsync(
            paymentHistory,
            input.TransactionId,
            input.PaymentReceived,
            input.PaymentDate,
            input.Method
        );

        await _electricityPaymentHistoryRepository.UpdateAsync(paymentHistory);
    }

    [Authorize(BillingPermissions.ElectricityPaymentHistories.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var paymentHistory = await _electricityPaymentHistoryRepository.GetAsync(id);
        await _electricityPaymentHistoryRepository.DeleteAsync(paymentHistory);
    }

    public async Task<IRemoteStreamContent> DownloadImportTemplateAsync()
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("Template");

        sheet.Cell(1, 1).Value = _localizer["ElectricityBillId"].Value;
        sheet.Cell(1, 2).Value = _localizer["TransactionId"].Value;
        sheet.Cell(1, 3).Value = _localizer["PaymentReceived"].Value;
        sheet.Cell(1, 4).Value = _localizer["PaymentDate"].Value;
        sheet.Cell(1, 5).Value = _localizer["PaymentMethod"].Value; // Cash, BankTransfer, Online etc.

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(
            stream,
            "ElectricityPaymentImportTemplate.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    public async Task<ImportResultDto> ImportExcelFileAsync(IFormFile file)
    {
        var result = new ImportResultDto();
        var rows = new List<ElectricityPaymentExcelDto>();

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

                if (!Guid.TryParse(billIdString, out var billId))
                {
                    result.Errors.Add($"{_localizer["Row"]} {row}: {_localizer["InvalidElectricityBillId"]} ({billIdString})");
                    continue;
                }

                try
                {
                    var dto = new ElectricityPaymentExcelDto
                    {
                        ElectricityBillId = billId,
                        TransactionId = sheet.Cell(row, 2).GetValue<string>()?.Trim(),
                        PaymentReceived = sheet.Cell(row, 3).GetValue<decimal>(),
                        PaymentDate = sheet.Cell(row, 4).GetValue<DateTime>(),
                        Method = sheet.Cell(row, 5).GetValue<string>()?.Trim(),
                    };

                    rows.Add(dto);
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"{_localizer["Row"]} {row}: {_localizer["Invaliddataformat"]} ({ex.Message})");
                }
            }
        }

        if (rows.Count == 0)
            throw new UserFriendlyException(_localizer["Excelcontainsnovalidrows."]);

        foreach (var row in rows)
        {
            try
            {
                if (row.ElectricityBillId == Guid.Empty)
                    throw new UserFriendlyException(_localizer["ElectricityBillIdisrequired."]);

                if (string.IsNullOrWhiteSpace(row.TransactionId))
                    throw new UserFriendlyException(_localizer["TransactionIdisrequired."]);

                if (row.PaymentReceived <= 0)
                    throw new UserFriendlyException(_localizer["PaymentReceivedmustbegreaterthanzero."]);

                if (string.IsNullOrWhiteSpace(row.Method))
                    throw new UserFriendlyException(_localizer["Paymentmethodisrequired."]);

                if (!Enum.TryParse<PaymentMethod>(row.Method, true, out var paymentMethod))
                {
                    result.Errors.Add($"{_localizer["Invalidpaymentmethod"]}: {row.Method}");
                    continue;
                }

                var bill = await _electricityBillRepository.FindAsync(row.ElectricityBillId);
                if (bill == null)
                {
                    result.Errors.Add($"{_localizer["Electricitybillnotfound"]}: {row.ElectricityBillId}");
                    continue;
                }

                // Create payment
                var history = await _electricityPaymentHistoryManager.CreateAsync(
                    row.ElectricityBillId,
                    row.TransactionId,
                    row.PaymentReceived,
                    row.PaymentDate,
                    paymentMethod
                );

                await _electricityPaymentHistoryRepository.InsertAsync(history);

                result.SuccessCount++;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"{_localizer["ErrorprocessingBill"]} {row.ElectricityBillId}: {ex.Message}");
            }
        }

        return result;
    }


}
