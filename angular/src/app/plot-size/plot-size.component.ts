import { PagedResultDto, ListService } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PlotSizeDto, GetPlotSizeListDto, PlotUnit, PlotSizeService, UpdatePlotSizeDto, CreatePlotSizeDto, plotUnitOptions } from '../proxy/plot-sizes';

@Component({
  selector: 'app-plot-size',
  standalone: false,
  templateUrl: './plot-size.component.html',
  styleUrl: './plot-size.component.scss',
  providers: [ListService],
})
export class PlotSizeComponent implements OnInit {
 plotSizes = { items: [], totalCount: 0 } as PagedResultDto<PlotSizeDto>;
  form: FormGroup;
  isModalOpen = false;
  showFilter = false;
  selectedPlotSize = {} as PlotSizeDto;
  filters = {} as GetPlotSizeListDto;
  plotUnits = plotUnitOptions;

  constructor(
    public readonly list: ListService,
    private plotSizeService: PlotSizeService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private toaster: ToasterService
  ) {}

  ngOnInit(): void {
    const streamCreator = query => this.plotSizeService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe(res => (this.plotSizes = res));
  }

  private buildForm() {
    this.form = this.fb.group({
      sizeName: [this.selectedPlotSize.sizeName || '', Validators.required],
      area: [this.selectedPlotSize.area || null, Validators.required],
      unit: [this.selectedPlotSize.unit || null, Validators.required],
      length: [this.selectedPlotSize.length || null],
      width: [this.selectedPlotSize.width || null],
      description: [this.selectedPlotSize.description || null],
      isActive: [this.selectedPlotSize.isActive ?? true],
    });
  }

  createPlotSize() {
    this.selectedPlotSize = {} as PlotSizeDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editPlotSize(id: string) {
    this.plotSizeService.get(id).subscribe(plot => {
      this.selectedPlotSize = plot;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) return;
    const dto = this.form.getRawValue();

    if (this.selectedPlotSize.id) {
      this.plotSizeService.update(this.selectedPlotSize.id, dto as UpdatePlotSizeDto).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::SuccessfullyUpdated');
      });
    } else {
      this.plotSizeService.create(dto as CreatePlotSizeDto).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::SuccessfullyCreated');
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.plotSizeService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  clearFilters() {
    this.filters = {} as GetPlotSizeListDto;
    this.list.get();
  }
}
