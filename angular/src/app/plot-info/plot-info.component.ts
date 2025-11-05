import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PlotInfoDto, GetPlotInfoListDto, PlotInfoService, plotStatusOptions } from '../proxy/plot-infos';
import { PhaseLookUp, PhaseService } from '../proxy/phases';
import { ConsumerPersonalInfoLookupDto, ConsumerPersonalInfoService } from '../proxy/consumer-personal-infos';
import { BlockLookupDto, BlockService } from '../proxy/blocks';

@Component({
  selector: 'app-plot-info',
  standalone: false,
  templateUrl: './plot-info.component.html',
  styleUrl: './plot-info.component.scss',
  providers: [ListService],
})
export class PlotInfoComponent implements OnInit{
  plots = { items: [], totalCount: 0 } as PagedResultDto<PlotInfoDto>;
  filters = {} as GetPlotInfoListDto;
  showFilter = false;
  plotStatus = plotStatusOptions;
  blocks = [] as BlockLookupDto[];
  phases = [] as PhaseLookUp[];
  consumers = [] as ConsumerPersonalInfoLookupDto[];

  constructor(
    public readonly list: ListService,
    private plotService: PlotInfoService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private phaseService: PhaseService,
    private blockService: BlockService,
   private consumerService: ConsumerPersonalInfoService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const streamCreator = (query) => this.plotService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.plots = res));
    this.getBlocks();
    this.getPhases();
    this.getConsumer();
  }

  getConsumer() {
    this.consumerService.consumerPersonalInfoLookup().subscribe((res) => {
      this.consumers = res;
    });
  }
  
  getPhases() {
    this.phaseService.getPhaseLookUp().subscribe((res) => {
      this.phases = res;
    });
  }


  getBlocks() {
    this.blockService.getBlockLookup().subscribe((res) => {
      this.blocks = res;
    });
  }

  editPlot(id: string) {
    this.router.navigate(['/createPlotInfos'], { queryParams: { id, edit: true } });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.plotService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  viewPlot(id: string) {
    this.router.navigate(['/createPlotInfos'], { queryParams: { id, view: true } });
  }

  clearFilters() {
    this.filters = {} as GetPlotInfoListDto;
    this.list.get();
  }

  navigateToCreate() {
    this.router.navigate(['/createPlotInfos']);
  }
}
