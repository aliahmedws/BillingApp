import { ListService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MeterInfoDto, meterTypeOptions, meterStatusOptions, MeterInfoService, MeterStatus, CreateMeterInfoDto, UpdateMeterInfoDto, meterCategoryOptions } from 'src/app/proxy/meter-infos';
import { PhaseLookUp, PhaseService } from 'src/app/proxy/phases';
import { PlotInfoLookupDto, PlotInfoService } from 'src/app/proxy/plot-infos';

@Component({
  selector: 'app-create-meter-info',
  standalone: false,
  templateUrl: './create-meter-info.component.html',
  styleUrl: './create-meter-info.component.scss',
  providers: [ListService]
})
export class CreateMeterInfoComponent implements OnInit{
 form: FormGroup;
  isViewMode = false;
  isEditMode = false;
  id: string | null = null;
  meter = {} as MeterInfoDto;
  meterTypes = meterTypeOptions;
  meterCategory = meterCategoryOptions;
  meterStatuses = meterStatusOptions;
  phases = [] as PhaseLookUp[];
  plots = [] as PlotInfoLookupDto[];
  selectedMeterInfo = {} as MeterInfoDto;

  constructor(
    private fb: FormBuilder,
    private meterService: MeterInfoService,
    private phaseService: PhaseService,
    private plotService: PlotInfoService,
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
      this.meterService.get(this.id).subscribe((data) => {
        this.meter = data;

        const formatInstallationDate = data.installationDate ? data.installationDate.split('T')[0] : null;

        this.form.patchValue({...data, installationDate: formatInstallationDate});
        if (this.isViewMode) this.form.disable();
      });
    }

    this.getPhases();
    this.getPlots();
  }

  getPhases() {
    this.phaseService.getPhaseLookUp().subscribe((res) => (this.phases = res));
  }

  getPlots() {
    this.plotService.getPlotLookUp().subscribe((res) => (this.plots = res));
  }

  buildForm() {
    this.form = this.fb.group({
      meterNo: [this.selectedMeterInfo.meterNo || '', [Validators.required, Validators.maxLength(64)]],
      meterType: [this.selectedMeterInfo.meterType || null, Validators.required],
      meterCategory: [this.selectedMeterInfo.meterCategory || null, Validators.required],
      meterStatus: [ this.selectedMeterInfo.meterStatus === MeterStatus.Active, Validators.required],
      installationDate: [ this.selectedMeterInfo.installationDate || null, Validators.required],
      initialReading: [ this.selectedMeterInfo.initialReading === 0 || 0, [Validators.required, Validators.min(0)]],
      phaseId: [ this.selectedMeterInfo.phaseId || null, Validators.required],
      plotId: [ this.selectedMeterInfo.plotId || null, Validators.required],
      remarks: [ this.selectedMeterInfo.remarks || '', Validators.maxLength(512)],
    });
  }

  save() {
    if (this.isViewMode) {
      this.backToList();
      return;
    }
    if (this.form.invalid) return;

    const dto = this.form.value as CreateMeterInfoDto | UpdateMeterInfoDto;

    if (this.isEditMode && this.id) {
      this.meterService.update(this.id, dto).subscribe(() => {
        this.toaster.success('::SuccessfullyUpdated');
        this.backToList();
      });
    } else {
      this.meterService.create(dto).subscribe(() => {
        this.toaster.success('::SuccessfullyCreated');
        this.backToList();
      });
    }
  }

  backToList() {
    this.router.navigate(['/meterInfos']);
  }
}
