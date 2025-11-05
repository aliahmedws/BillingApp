import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BlockLookupDto, BlockService } from 'src/app/proxy/blocks';
import { ConsumerPersonalInfoLookupDto, ConsumerPersonalInfoService } from 'src/app/proxy/consumer-personal-infos';
import { PhaseLookUp, PhaseService } from 'src/app/proxy/phases';
import { PlotInfoDto, PlotInfoService, PlotStatus, CreatePlotInfoDto, UpdatePlotInfoDto, plotStatusOptions } from 'src/app/proxy/plot-infos';
import { PlotSizeLookupDto, PlotSizeService } from 'src/app/proxy/plot-sizes';
import { plotTypeOptions } from 'src/app/proxy/plot-types';

@Component({
  selector: 'app-create-plot-info',
  standalone: false,
  templateUrl: './create-plot-info.component.html',
  styleUrl: './create-plot-info.component.scss'
})
export class CreatePlotInfoComponent implements OnInit {
 form: FormGroup;
  isViewMode = false;
  isEditMode = false;
  id: string | null = null;
  plot = {} as PlotInfoDto;
  plotTypes = plotTypeOptions;
  plotStatus = plotStatusOptions;
  plotSizes = [] as PlotSizeLookupDto[];
  blocks = [] as BlockLookupDto[];
  phases = [] as PhaseLookUp[];
  consumers = [] as ConsumerPersonalInfoLookupDto[];

  selectedPlotInformation = {} as PlotInfoDto;

  constructor(
    private fb: FormBuilder,
    private plotService: PlotInfoService,
    private plotSizeService: PlotSizeService,
    private phaseService: PhaseService,
    private blockService: BlockService,
    private consumerService: ConsumerPersonalInfoService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.isViewMode = this.route.snapshot.queryParamMap.get('view') === 'true';
    this.isEditMode = this.route.snapshot.queryParamMap.get('edit') === 'true';

    this.buildForm();

    if (this.id) {
      this.plotService.get(this.id).subscribe((data) => {
        this.plot = data;
        this.form.patchValue(data);
        if (this.isViewMode) {
          this.form.disable();
        }
      });
    }
    this.getPlotSizes();
    this.getBlocks();
    this.getPhases();
    this.getConsumer();
  }

  getConsumer() {
    this.consumerService.consumerPersonalInfoLookup().subscribe((res) => {
      this.consumers = res;
    });
  }

  getPlotSizes() {
    this.plotSizeService.getPlotSizeLookup().subscribe((res) => {
       this.plotSizes = res;
    });
  }

  getBlocks() {
    this.blockService.getBlockLookup().subscribe((res) => {
      this.blocks = res;
    });
  }

  getPhases() {
    this.phaseService.getPhaseLookUp().subscribe((res) => {
      this.phases = res;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      plotNo: [this.selectedPlotInformation.plotNo || '', [Validators.required, Validators.maxLength(64)]],
      streetNo: [this.selectedPlotInformation.streetNo || '', [Validators.required, Validators.maxLength(64)]],
      plotType: [this.selectedPlotInformation.plotType || null, Validators.required],
      plotSizeId: [this.selectedPlotInformation.plotSizeId || null, Validators.required],
      blockId: [this.selectedPlotInformation.blockId || null, Validators.required],
      phaseId: [this.selectedPlotInformation.phaseId || null, Validators.required],
      consumerId: [this.selectedPlotInformation.consumerId || null],
      status: [this.selectedPlotInformation.status || PlotStatus.Available, Validators.required],
      remarks: [this.selectedPlotInformation.remarks || '', Validators.maxLength(512)],
    });
  }

  save() {
    if (this.isViewMode) {
      this.backToList();
      return;
    }
    if (this.form.invalid) return;

    const dto = this.form.value as CreatePlotInfoDto | UpdatePlotInfoDto;

    if (this.isEditMode && this.id) {
      this.plotService.update(this.id, dto).subscribe(() => {
        this.toaster.success('::SuccessfullyUpdated');
        this.backToList();
      });
    } else {
      this.plotService.create(dto).subscribe(() => {
        this.toaster.success('::SuccessfullyCreated');
        this.backToList();
      });
    }
  }

  backToList() {
    this.router.navigate(['/plotInfos']);
  }
}
