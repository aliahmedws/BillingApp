import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { IescoChargeDto, IescoChargeService, UpdateIescoChargeDto } from '../proxy/iesco-charges';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-iescocharge',
  standalone: false,
  templateUrl: './iescocharge.component.html',
  styleUrl: './iescocharge.component.scss',
  providers: [ListService],
})
export class IescochargeComponent implements OnInit {
  iescoCharges = { items: [], totalCount: 0 } as PagedResultDto<IescoChargeDto>;
  form: FormGroup;
  isModalOpen = false;
  selectedIescoCharge = {} as IescoChargeDto;
  errorMessage: string | null = null;
  
  constructor(
    public readonly list: ListService,
    private iescoChargeService: IescoChargeService,
    private fb: FormBuilder,
    private toaster: ToasterService
  ) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.iescoChargeService.getList(query);
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.iescoCharges = res));
  }

  private buildForm() {
    this.form = this.fb.group({
      totalEnergyCharges: [this.selectedIescoCharge.totalEnergyCharges ?? 0],
      iescoFixCharges: [this.selectedIescoCharge.iescoFixCharges ?? 0],
      serviceRent: [this.selectedIescoCharge.serviceRent ?? 0],
      varFpa: [this.selectedIescoCharge.varFpa ?? 0],
      qtrTariffAdj: [this.selectedIescoCharge.qtrTariffAdj ?? 0]
      });
  }

  editIescoCharge(id: string) {
    this.iescoChargeService.get(id).subscribe((iescoCharge) => {
      this.selectedIescoCharge = iescoCharge;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
    if (!this.form.valid) 
      return;
    const dto = this.form.value;
    if (this.selectedIescoCharge.id) {
      this.iescoChargeService.update(this.selectedIescoCharge.id, dto as UpdateIescoChargeDto).subscribe({
      next: () => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::SuccessfullyUpdated');
    }, 
    error: (err) => {
      if (err && err.error && err.error.error && err.error.error.message) {
        this.toaster.error(err.error.error.message);
      } else {
        this.toaster.error('An unexpected error occurred.');
      }
    }
    });
    }
}
}
