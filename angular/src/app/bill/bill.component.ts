import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import {
  billStatusOptions,
  GenerateMaintenanceBillsDto,
  GetMaintenanceBillListDto,
  MaintenanceBillDto,
  MaintenanceBillService,
} from '../proxy/maintenance-bills';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { ListService, PagedResultDto } from '@abp/ng.core';
import {
  ConsumerPersonalInfoLookupDto,
  ConsumerPersonalInfoService,
} from '../proxy/consumer-personal-infos';
import { PlotInfoLookupDto, PlotInfoService } from '../proxy/plot-infos';
import { MaintenancePaymentHistoryService } from '../proxy/maintenance-payment-histories';
import { PaymentImportService } from 'src/custom-services/payment-import/payment-import.service';
import { MaintenanceBillTemplateService } from '../proxy/maintenance-bill-templates';

@Component({
  selector: 'app-bill',
  standalone: false,
  templateUrl: './bill.component.html',
  styleUrl: './bill.component.scss',
  providers: [ListService],
})
export class BillComponent implements OnInit {
  @ViewChild('fileInput', { static: false }) fileInput!: ElementRef;
  bills = { items: [], totalCount: 0 } as PagedResultDto<MaintenanceBillDto>;

  generating = false;
  generateModalVisible = false;
  showFilter = false;
  filters = {} as GetMaintenanceBillListDto;

  consumerLookup: ConsumerPersonalInfoLookupDto[] = [];
  plotLookup: PlotInfoLookupDto[] = [];
  billStatus = billStatusOptions;

  generateModalLateSurcharge: number | null = 0;
  generateModalBillingMonth: string | null = null; // yyyy-MM
  generateModalIssueDate: string | null = null; // yyyy-MM-dd
  generateModalDueDate: string | null = null;

  constructor(
    public readonly list: ListService,
    private maintenanceBillService: MaintenanceBillService,
    private consumerService: ConsumerPersonalInfoService,
    private plotService: PlotInfoService,
    private maintenancePaymentHistoryService: MaintenancePaymentHistoryService,
    private maintenanceBillTemplateService: MaintenanceBillTemplateService,
    private importService: PaymentImportService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const streamCreator = query =>
      this.maintenanceBillService.getList({ ...query, ...this.filters });

    this.list.hookToQuery(streamCreator).subscribe(res => (this.bills = res));

    this.loadLookups();
  }

  loadLookups() {
    this.consumerService.consumerPersonalInfoLookup().subscribe(res => (this.consumerLookup = res));

    this.plotService.getPlotLookUp().subscribe(res => (this.plotLookup = res));
  }

  navigateToCreateMaintenance() {
    this.router.navigate(['/create-maintenance-bills']);
  }

  openGenerateModal() {
    const now = new Date();

    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');

    this.generateModalLateSurcharge = 0;
    this.generateModalBillingMonth = `${now.getFullYear()}-${month}`;
    this.generateModalIssueDate = `${now.getFullYear()}-${month}-${day}`;
    this.generateModalDueDate = `${now.getFullYear()}-${month}-${day}`;

    this.generateModalVisible = true;
  }

  handleGenerateCancel() {
    if (this.generating) {
      return;
    }
    this.generateModalVisible = false;
  }

  handleGenerateOk() {
    if (this.generating) {
      return;
    }

    const value = this.generateModalLateSurcharge ?? 0;

    if (value < 0) {
      this.toaster.warn('::LatePaymentSurchargeCannotBeNegative');
      return;
    }

    if (
      !this.generateModalBillingMonth ||
      !this.generateModalIssueDate ||
      !this.generateModalDueDate
    ) {
      this.toaster.warn('::GenerateMaintenanceBillsMissingDates');
      return;
    }

    const [bmYear, bmMonth] = this.generateModalBillingMonth.split('-').map(x => +x);
    const billingMonthDate = new Date(bmYear, bmMonth - 1, 1);

    const issueDate = new Date(this.generateModalIssueDate);
    const dueDate = new Date(this.generateModalDueDate);

    if (dueDate < issueDate) {
      this.toaster.warn('::DueDateMustBeAfterIssueDate');
      return;
    }

    this.confirmation
      .warn('::AreYouSureToGenerateMaintenanceBills', '::AreYouSure')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.generateMaintenanceBills(billingMonthDate, issueDate, dueDate, value);
        }
      });
  }

  generateMaintenanceBills(
    billingMonth: Date,
    issueDate: Date,
    dueDate: Date,
    latePaymentSurcharge: number
  ) {
    const input: GenerateMaintenanceBillsDto = {
      billingMonth: billingMonth.toISOString(),
      issueDate: issueDate.toISOString(),
      dueDate: dueDate.toISOString(),
      latePaymentSurcharge,
    };

    this.generating = true;

    this.maintenanceBillService.generate(input).subscribe(() => {
      this.list.get();
      this.toaster.success('::MaintenanceBillsGeneratedSuccessfully');
      this.generateModalVisible = false;
      this.generating = false;
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.maintenanceBillService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  clearFilters() {
    this.filters = {} as GetMaintenanceBillListDto;
    this.list.get();
  }

  viewMaintenanceBill(id: string) {
    this.router.navigate(['create-maintenance-bills'], {
      queryParams: { id: id, mode: 'view' },
    });
  }

  downloadTemplate() {
    this.maintenancePaymentHistoryService.downloadImportTemplate().subscribe(blob => {
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = url;
      a.download = 'PaymentImportTemplate.xlsx';
      a.click();

      window.URL.revokeObjectURL(url);
    });
  }

  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) {
      this.toaster.warn('::Nofileselected.');
      return;
    }

    const file = input.files[0];

    const formData = new FormData();
    formData.append('file', file);

    this.importExcel(formData);
  }

  importExcel(formData: FormData) {
    this.importService.importExcelFile(formData).subscribe(res => {
      this.toaster.success('::ImportedSuccessfully.');
    });
  }

  downloadExcel() {
    const input = {
      ...this.filters,
      maxResultCount: 1000,
    };

    this.maintenanceBillService.getListAsExcelFile(input).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'MaintenancePaymentReport.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }

  printFiltered() {
    const input = {
      ...this.filters,
      skipCount: 0,
      maxResultCount: 1000,
      sorting: 'CreationTime Desc',
    };

    this.maintenanceBillTemplateService.printFiltered(input).subscribe({
      next: (response: any) => {
        const blob = new Blob([response], { type: 'text/html' });
        const url = window.URL.createObjectURL(blob);
        window.open(url, '_blank'); // open print preview
      },
      error: () => {
        this.toaster.error('::FailedToPrint');
      },
    });
  }
}
