import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { GovtChargeDto, GovtChargeService, UpdateGovtChargeDto } from '../proxy/govt-charges';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-govtcharge',
  standalone: false,
  templateUrl: './govtcharge.component.html',
  styleUrl: './govtcharge.component.scss',
  providers: [ListService],
})
export class GovtchargeComponent implements OnInit {
  govtCharges = { items: [], totalCount: 0 } as PagedResultDto<GovtChargeDto>;
  form: FormGroup;
  isModalOpen = false;
  showFilter = false;
  selectedGovtCharge = {} as GovtChargeDto;
  errorMessage: string | null = null;

  constructor(
    public readonly list: ListService,
    private govtChargeService: GovtChargeService,
    private fb: FormBuilder,
    private toaster: ToasterService
  ) { }

  ngOnInit(): void {
  const streamCreator = () => this.govtChargeService.getList();
  this.list.hookToQuery(streamCreator).subscribe((res) => (this.govtCharges = res));
}

  private buildForm() {
    this.form = this.fb.group({
      ed: [this.selectedGovtCharge.ed || null],
      tvFee: [this.selectedGovtCharge.tvFee || null],
      gst: [this.selectedGovtCharge.gst || null],
      incomeTax: [this.selectedGovtCharge.incomeTax || null],
      extraTax: [this.selectedGovtCharge.extraTax || null],
      furtherTax: [this.selectedGovtCharge.furtherTax || null],
      njSurcharge: [this.selectedGovtCharge.njSurcharge || null],
      salesTax: [this.selectedGovtCharge.salesTax || null],
      fcSurcharge: [this.selectedGovtCharge.fcSurcharge || null],
      trSurcharge: [this.selectedGovtCharge.trSurcharge || null],
      taxOnFpa: [this.selectedGovtCharge.taxOnFpa || null]
    });
  }

 
  editGovtCharge(id: string) {
    this.govtChargeService.get(id).subscribe((govtCharge) => {
      this.selectedGovtCharge = govtCharge;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
  if (this.form.invalid) return;

  const dto = this.form.getRawValue();

  if (this.selectedGovtCharge.id) {
    this.govtChargeService.update(this.selectedGovtCharge.id, dto as UpdateGovtChargeDto).subscribe({
      next: () => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.info('Successfully updated');
      },
      error: (err) => {
        if (err && err.error && err.error.error && err.error.error.message) {
          this.toaster.error(err.error.error.message);
        } else {
          this.toaster.error('An unexpected error occurred.');
        }
      },
    });
  }
}
}
