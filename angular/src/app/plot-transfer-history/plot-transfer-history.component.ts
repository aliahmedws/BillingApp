import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConsumerPersonalInfoLookupDto, ConsumerPersonalInfoService } from '../proxy/consumer-personal-infos';
import { PlotInfoLookupDto, PlotInfoService } from '../proxy/plot-infos';
import { PlotTransferHistoryDto, GetPlotTransferHistoryListDto, transferTypeOptions, PlotTransferHistoryService } from '../proxy/plot-transfer-histories';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-plot-transfer-history',
  standalone: false,
  templateUrl: './plot-transfer-history.component.html',
  styleUrl: './plot-transfer-history.component.scss',
  providers: [ListService]
})
export class PlotTransferHistoryComponent implements OnInit{
  transfers = { items: [], totalCount: 0 } as PagedResultDto<PlotTransferHistoryDto>;
  filters = {} as GetPlotTransferHistoryListDto;
  showFilter = false;
  transferTypes = transferTypeOptions;
  transferStatus = transferTypeOptions;
  plots = [] as PlotInfoLookupDto[];
  consumers = [] as ConsumerPersonalInfoLookupDto[];

  constructor(
    public readonly list: ListService,
    private transferService: PlotTransferHistoryService,
    private plotService: PlotInfoService,
    private consumerService: ConsumerPersonalInfoService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    const streamCreator = (query) => this.transferService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.transfers = res));
    this.getPlots();
    this.getConsumers();
  }

  getPlots() {
    this.plotService.getPlotLookUp().subscribe((res) => (this.plots = res));
  }

  getConsumers() {
    this.consumerService.consumerPersonalInfoLookup().subscribe((res) => (this.consumers = res));
  }

  editTransfer(id: string) {
    this.router.navigate(['/CreatePlotTransferHistories'], { queryParams: { id, edit: true } });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.transferService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  viewTransfer(id: string) {
    this.router.navigate(['/CreatePlotTransferHistories'], { queryParams: { id, view: true } });
  }

  clearFilters() {
    this.filters = {} as GetPlotTransferHistoryListDto;
    this.list.get();
  }

  navigateToCreate() {
    this.router.navigate(['/CreatePlotTransferHistories']);
  }

  
}
