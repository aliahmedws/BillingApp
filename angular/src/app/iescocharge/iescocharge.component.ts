import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { IescoChargeDto, IescoChargeService, UpdateIescoChargeDto } from '../proxy/iesco-charges';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';

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
      totalEnergyCharges: [this.selectedIescoCharge.totalEnergyCharges || null],
      iescoFixCharges: [this.selectedIescoCharge.iescoFixCharges || null],
      serviceRent: [this.selectedIescoCharge.serviceRent || null],
      varFpa: [this.selectedIescoCharge.varFpa || null],
      qtrTariffAdj: [this.selectedIescoCharge.qtrTariffAdj || null]
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
        this.toaster.info('::SuccessfullyUpdated');
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
