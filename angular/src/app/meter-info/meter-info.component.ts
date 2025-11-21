import { PagedResultDto, ListService } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MeterInfoDto, GetMeterInfoListDto, meterTypeOptions, meterStatusOptions, MeterInfoService, meterCategoryOptions } from '../proxy/meter-infos';
import { PhaseLookUp, PhaseService } from '../proxy/phases';
import { PlotInfoLookupDto, PlotInfoService } from '../proxy/plot-infos';

@Component({
  selector: 'app-meter-info',
  standalone: false,
  templateUrl: './meter-info.component.html',
  styleUrl: './meter-info.component.scss',
  providers: [ListService]
})
export class MeterInfoComponent implements OnInit{
  meters = { items: [], totalCount: 0 } as PagedResultDto<MeterInfoDto>;
  filters = {} as GetMeterInfoListDto;
  showFilter = false;
  meterTypes = meterTypeOptions;
  meterCategory = meterCategoryOptions;
  meterStatus = meterStatusOptions;
  phases = [] as PhaseLookUp[];
  plots = [] as PlotInfoLookupDto[];

  constructor(
    public readonly list: ListService,
    private meterService: MeterInfoService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private phaseService: PhaseService,
    private plotService: PlotInfoService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const streamCreator = (query) => this.meterService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.meters = res));
    this.getPhases();
    this.getPlots();
  }

  getPhases() {
    this.phaseService.getPhaseLookUp().subscribe((res) => (this.phases = res));
  }

  getPlots() {
    this.plotService.getPlotLookUp().subscribe((res) => (this.plots = res));
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.meterService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  viewMeter(id: string) {
    this.router.navigate(['/createMeterInfos'], { queryParams: { id, view: true } });
  }

  clearFilters() {
    this.filters = {} as GetMeterInfoListDto;
    this.list.get();
  }

  navigateToCreate() {
    this.router.navigate(['/createMeterInfos']);
  }
}
