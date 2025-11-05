import { ListService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ConsumerPersonalInfoLookupDto, ConsumerPersonalInfoService } from 'src/app/proxy/consumer-personal-infos';
import { PlotInfoLookupDto, PlotInfoService } from 'src/app/proxy/plot-infos';
import { PlotTransferHistoryDto, transferTypeOptions, PlotTransferHistoryService, CreatePlotTransferHistoryDto, UpdatePlotTransferHistoryDto } from 'src/app/proxy/plot-transfer-histories';

@Component({
  selector: 'app-create-plot-transfer-history',
  standalone: false,
  templateUrl: './create-plot-transfer-history.component.html',
  styleUrl: './create-plot-transfer-history.component.scss',
  providers: [ListService],
})
export class CreatePlotTransferHistoryComponent implements OnInit{
 form: FormGroup;
  isViewMode = false;
  isEditMode = false;
  id: string | null = null;

  transfer = {} as PlotTransferHistoryDto;
  transferTypes = transferTypeOptions;
  plots = [] as PlotInfoLookupDto[];
  consumers = [] as ConsumerPersonalInfoLookupDto[];

  selectedTransfer = {} as PlotTransferHistoryDto;

  constructor(
    private fb: FormBuilder,
    private transferService: PlotTransferHistoryService,
    private plotService: PlotInfoService,
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
    this.form.get('plotId')?.valueChanges.subscribe((plotId) => {
      this.onPlotChange(plotId);
    });

    if (this.id) {
      this.transferService.get(this.id).subscribe((data) => {
        this.transfer = data;
        const formattedDate = data.transferDate ? data.transferDate.split('T')[0] : null;
        this.form.patchValue({ ...data, transferDate: formattedDate });
        if (this.isViewMode) this.form.disable();
      });
    }

    this.getPlots();
    this.getConsumers();
  }

  onPlotChange(plotId: string) {
    if(!plotId) {
      this.form.get('fromConsumerId')?.reset();
      return;
    }

    this.plotService.getPlotOwner(plotId).subscribe((res) => {
      if(res && res.consumerId)
      {
        this.form.get('fromConsumerId')?.setValue(res.consumerId);
        this.form.get('fromConsumerId')?.disable();
      }
      else {
        this.form.get('fromConsumerId').reset();
      }
    });
  }

  getPlots() {
    this.plotService.getPlotLookUp().subscribe((res) => (this.plots = res));
  }

  getConsumers() {
    this.consumerService.consumerPersonalInfoLookup().subscribe((res) => (this.consumers = res));
  }

  buildForm() {
    this.form = this.fb.group({
      plotId: [this.selectedTransfer.plotId || null, Validators.required],
      fromConsumerId: [this.selectedTransfer.fromConsumerId || null, Validators.required],
      toConsumerId: [this.selectedTransfer.toConsumerId || null, Validators.required],
      transferDate: [this.selectedTransfer.transferDate || null, Validators.required],
      transferType: [this.selectedTransfer.transferType || null, Validators.required],
      registryNo: [this.selectedTransfer.registryNo || '', [Validators.required, Validators.maxLength(100)]],
      considerationAmount: [this.selectedTransfer.considerationAmount || 0, [Validators.required, Validators.min(0)]],
      remarks: [this.selectedTransfer.remarks || '', Validators.maxLength(512)],
    });
  }

  save() {
    if (this.isViewMode) {
      this.backToList();
      return;
    }
    if (this.form.invalid) return;

    this.form.get('fromConsumerId')?.enable({ emitEvent: false });

    const dto = this.form.value as CreatePlotTransferHistoryDto | UpdatePlotTransferHistoryDto;

    if (this.isEditMode && this.id) {
      this.transferService.update(this.id, dto).subscribe(() => {
        this.toaster.success('::SuccessfullyUpdated');
        this.backToList();
      });
    } else {
      this.transferService.create(dto).subscribe(() => {
        this.toaster.success('::SuccessfullyCreated');
        this.backToList();
      });
    }
  }

  backToList() {
    this.router.navigate(['/plotTransferHistories']);
  }
}
