import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ElectricityBillDto, GetElectricityBillListDto, ElectricityBillService } from 'src/app/proxy/electricity-bills';
import { billStatusOptions } from 'src/app/proxy/maintenance-bills';
import { MeterInfoDto } from 'src/app/proxy/meter-infos';

@Component({
  selector: 'app-electricity-bill',
  standalone: false,
  templateUrl: './electricity-bill.component.html',
  styleUrl: './electricity-bill.component.scss',
  providers: [ListService]
})
export class ElectricityBillComponent implements OnInit{
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


}