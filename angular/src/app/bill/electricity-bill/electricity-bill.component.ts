import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ElectricityBillDto, GetElectricityBillListDto, ElectricityBillService } from 'src/app/proxy/electricity-bills';
import { ElectricityPaymentHistoryService } from 'src/app/proxy/electricity-payment-histories';
import { billStatusOptions } from 'src/app/proxy/maintenance-bills';
import { MeterInfoDto } from 'src/app/proxy/meter-infos';
import { PaymentImportService } from 'src/custom-services/payment-import/payment-import.service';

@Component({
  selector: 'app-electricity-bill',
  standalone: false,
  templateUrl: './electricity-bill.component.html',
  styleUrl: './electricity-bill.component.scss',
  providers: [ListService]
})
export class ElectricityBillComponent implements OnInit {
  @ViewChild('fileInput', { static: false }) fileInput!: ElementRef;
  bills = { items: [], totalCount: 0} as PagedResultDto<ElectricityBillDto>;

  filters = {} as GetElectricityBillListDto;
  showFilter = false;

  meterLookup: MeterInfoDto[] = [];
  billStatus = billStatusOptions;

  generating = false;
  generateModalVisible = false;

  generateModalBillingMonth: string | null = null;
  generateModalIssueDate: string | null = null;
  generateModalDueDate: string | null = null;
  generateModalLateSurcharge: number | null = 0;

  constructor(
    public readonly list: ListService,
    private electricityService: ElectricityBillService,
    private electricityPaymentHistoryService: ElectricityPaymentHistoryService,
    private importExcelService: PaymentImportService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private router: Router
  ) {}

  ngOnInit(): void {

    const streamCreator = query => this.electricityService.getList({ ...query, ...this.filters });

    this.list.hookToQuery(streamCreator).subscribe(res => (this.bills = res));

    this.loadMeterLookup();
  }

  loadMeterLookup() {
    // this.meterService.getMeterLookup().subscribe(res => {
    //   this.meterLookup = res;
    // });
  }

  navigateToCreate() {
    this.router.navigate(['/create-electricity-bills']);
  }

  navigateToBulk() {
    this.router.navigate(['/bulk-electricity-bills']);
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.electricityService.delete(id).subscribe(() => {
            this.list.get();
            this.toaster.success('::SuccessfullyDeleted');
          });
        }
      });
  }

  clearFilters() {
    this.filters = {} as GetElectricityBillListDto;
    this.list.get();
  }

  view(id: string) {
    this.router.navigate(['/create-electricity-bills'], {
      queryParams: { id, mode: 'view' }
    });
  }

  downloadTemplate() {
    this.electricityPaymentHistoryService.downloadImportTemplate().subscribe(blob => {
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = url;
      a.download = 'ElectricityPaymentImportTemplate.xlsx';
      a.click();

      window.URL.revokeObjectURL(url);
    })
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
    this.importExcelService.importElectricityExcelFile(formData).subscribe(res => {
      this.toaster.success('::ImportedSuccessfully.')
      this.list.get();
    })
  }

  downloadExcel() {
    const input = {
      ...this.filters,
      maxResultCount: 1000
    }

    this.electricityService.getListAsExcelFile(input).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'ElectricityBillReport.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    })
  }


}