import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ElectricityBillService, CreateElectricityBillDto } from 'src/app/proxy/electricity-bills';
import { billStatusOptions } from 'src/app/proxy/maintenance-bills';
import { MeterInfoDto, MeterInfoLookupDto, MeterInfoService } from 'src/app/proxy/meter-infos';

@Component({
  selector: 'app-create-electricity-bill',
  standalone: false,
  templateUrl: './create-electricity-bill.component.html',
  styleUrl: './create-electricity-bill.component.scss'
})
export class CreateElectricityBillComponent implements OnInit{
 form!: FormGroup;

  mode: 'create' | 'edit' | 'view' = 'create';
  id: string | null = null;

  meters: MeterInfoLookupDto[] = [];
  billStatus = billStatusOptions;

  constructor(
    private fb: FormBuilder,
    private electricityService: ElectricityBillService,
    private meterService: MeterInfoService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.mode = (this.route.snapshot.queryParamMap.get('mode') as any) ?? 'create';

    this.buildForm();
    this.loadMeters();

    if (this.mode !== 'create' && this.id) {
      this.loadBill(this.id);
    }

    if (this.mode === 'view') this.form.disable();

    this.setupAutoCalculation();
  }

  buildForm() {
    this.form = this.fb.group({
      meterInfoId: [null, Validators.required],

      previousReading: [0, Validators.required],
      presentReading: [0, Validators.required],
      unitsConsumed: [{ value: 0, disabled: true }],

      meterReadingDate: [null, Validators.required],
      billingMonth: [null, Validators.required],
      issueDate: [null, Validators.required],
      dueDate: [null, Validators.required],

      currentMonthBill: [{ value: 0, disabled: true }],
      billAdjustment: [0],
      anyOtherCharges: [0],
      lpSurcharge: [0],

      totalPayable: [{ value: 0, disabled: true }],
      status: [this.billStatus[0]?.value ?? 0]
    });
  }

  loadMeters() {
    this.meterService.getMeterInfoLookup().subscribe(res => (this.meters = res));
  }

  loadBill(id: string) {
    this.electricityService.get(id).subscribe(res => {
      const formatted = {
        ...res,
        billingMonth: this.formatMonth(res.billingMonth),
        meterReadingDate: this.formatDate(res.meterReadingDate),
        issueDate: this.formatDate(res.issueDate),
        dueDate: this.formatDate(res.dueDate)
      };

      this.form.patchValue(formatted);
      if (this.mode === 'edit') this.form.enable();
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const dto = this.form.getRawValue() as CreateElectricityBillDto;

    if (this.mode === 'edit' && this.id) {
      this.electricityService.update(this.id, dto).subscribe(() => {
        this.toaster.success('Updated');
        this.back();
      });
    } else {
      this.electricityService.create(dto).subscribe(() => {
        this.toaster.success('Created');
        this.back();
      });
    }
  }

  back() {
    this.router.navigate(['/electricity-bills']);
  }

  enableEdit() {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { id: this.id, mode: 'edit' }
    });

    this.mode = 'edit';
    this.form.enable();
  }

  setupAutoCalculation() {
    this.form.valueChanges.subscribe(() => this.recalculate());
  }

  recalculate() {
    const val = (name: string) => Number(this.form.get(name)?.value ?? 0);

    const units = val('presentReading') - val('previousReading');
    this.form.get('unitsConsumed')?.setValue(units > 0 ? units : 0, {
      emitEvent: false
    });

    const total =
      val('currentMonthBill') +
      val('billAdjustment') +
      val('anyOtherCharges') +
      val('lpSurcharge');

    this.form.get('totalPayable')?.setValue(total, { emitEvent: false });
  }

  formatDate(date: string) {
    if (!date) return null;
    return new Date(date).toISOString().split('T')[0];
  }

  formatMonth(date: string) {
    if (!date) return null;
    const d = new Date(date);
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
  }

  print() {
    if (!this.id) {
      this.toaster.warn('::Nobillavailabletoprint');
      return;
    }

    this.router.navigate(['/print-electricity-bill'], {
      queryParams: { id: this.id }
    });
  }
}