import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ElectricityBillService, CreateElectricityBillDto, ElectricityBillDto } from 'src/app/proxy/electricity-bills';
import { ElectricityPaymentHistoryDto, ElectricityPaymentHistoryService } from 'src/app/proxy/electricity-payment-histories';
import { billStatusOptions } from 'src/app/proxy/maintenance-bills';
import { MeterInfoLookupDto, MeterInfoService } from 'src/app/proxy/meter-infos';

@Component({
  selector: 'app-create-electricity-bill',
  standalone: false,
  templateUrl: './create-electricity-bill.component.html',
  styleUrl: './create-electricity-bill.component.scss'
})
export class CreateElectricityBillComponent implements OnInit{
  form!: FormGroup;
  paymentForm: FormGroup;

  mode: 'create' | 'edit' | 'view' = 'create';
  id: string | null = null;

  meters: MeterInfoLookupDto[] = [];
  billStatus = billStatusOptions;

  latestPayment: any = null;

  isPaymentModalOpen = false;
  isBillFullyPaid = false;

  selectedElectricityBill = {} as ElectricityBillDto;
  selectedElectricityBillPaymentHistory = {} as ElectricityPaymentHistoryDto;

  constructor(
    private fb: FormBuilder,
    private electricityService: ElectricityBillService,
    private meterService: MeterInfoService,
    private electricityPaymentHistoryService: ElectricityPaymentHistoryService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.mode = (this.route.snapshot.queryParamMap.get('mode') as any) ?? 'create';

    this.buildForm();
    this.buildPaymentForm();
    this.loadMeters();

    if (this.mode !== 'create' && this.id) {
      this.loadBill(this.id);
    }

    if (this.mode === 'view') this.form.disable();

    this.setupAutoCalculation();

    this.form.get('meterInfoId')?.valueChanges.subscribe(meterId => {
      if(!meterId) return;

      if (this.mode === 'create') {
        this.loadLatestMeterReading(meterId);
      }
    })
  }

  buildForm() {
    this.form = this.fb.group({
      meterInfoId: [ this.selectedElectricityBill.meterInfoId || null, Validators.required],

      previousReading: [ this.selectedElectricityBill.previousReading || 0, Validators.required],
      presentReading: [ this.selectedElectricityBill.presentReading || 0, Validators.required],
      unitsConsumed: [{ value: this.selectedElectricityBill.consumedUnits || 0, disabled: true }],

      meterReadingDate: [ this.selectedElectricityBill.meterReadingDate || null, Validators.required],
      billingMonth: [ this.selectedElectricityBill.billingMonth || null, Validators.required],
      issueDate: [ this.selectedElectricityBill.issueDate || null, Validators.required],
      dueDate: [ this.selectedElectricityBill.dueDate || null, Validators.required],

      currentMonthBill: [{ value: this.selectedElectricityBill.currentMonthBill || 0, disabled: true }],
      billAdjustment: [ this.selectedElectricityBill.billAdjustment || 0],
      anyOtherCharges: [ this.selectedElectricityBill.anyOtherCharges || 0],
      lpSurcharge: [ this.selectedElectricityBill.lpSurcharge || 0],

      totalPayable: [{ value: this.selectedElectricityBill.payableDueDateAmount || 0, disabled: true }],
      status: [this.selectedElectricityBill.status ?? this.billStatus[0].value],
      arrears: [this.selectedElectricityBill.arrears || 0, Validators.required]
    });
  }

  buildPaymentForm() {
    this.paymentForm = this.fb.group({
      transactionId: [this.selectedElectricityBillPaymentHistory.transactionId || '', Validators.required],
      paymentReceived: [this.selectedElectricityBillPaymentHistory.paymentReceived || 0, Validators.required],
      paymentDate: [this.selectedElectricityBillPaymentHistory.paymentDate || '', Validators.required],
      method: [this.selectedElectricityBillPaymentHistory.method || 1, Validators.required]
    })
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
  const prev = Number(this.form.get('previousReading')?.value || 0);
  const pres = Number(this.form.get('presentReading')?.value || 0);

  const units = pres > prev ? pres - prev : 0;

  this.form.get('unitsConsumed')?.setValue(units, { emitEvent: false });

  this.calculateBillFromUnits(units);
  this.recalculateTotal();
  }


recalculateTotal() {
  const val = (name: string) => Number(this.form.get(name)?.value ?? 0);

  const total =
    val('currentMonthBill') +
    val('lpSurcharge') +
    val('anyOtherCharges') -
    val('billAdjustment');

  this.form.get('totalPayable')?.setValue(total, { emitEvent: false });
}

  formatDate(date: string) {
    if (!date) return null;
    return new Date(date).toISOString().split('T')[0];
  }

  formateDateForPayment(date: any) {
    if (!date) return null;
    const d = new Date(date);
    return d.toISOString().split('T')[0];
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

  calculateBillFromUnits(units: number) {
    if (units <= 0) {
      this.form.get('currentMonthBill')?.setValue(0, { emitEvent: false });
      this.recalculateTotal();
      return;
    }

    this.electricityService.calculateBill(units).subscribe(amount => {
      this.form.get('currentMonthBill')?.setValue(amount, { emitEvent: false });
      this.recalculateTotal();
    })
  }

  loadLatestMeterReading(meterId: string) {

  this.electricityService.getList({
    meterInfoId: meterId,
    maxResultCount: 1,
    sorting: "meterReadingDate DESC"
  })
  .subscribe(result => {

    const lastBill = result.items[0];

    if (lastBill) {
      this.form.patchValue({
        previousReading: lastBill.presentReading
      });
    } else {
      this.form.patchValue({
        previousReading: 0
      });
    }
  });
}

openPaymentModal() {
  if (!this.id) return;

  const payable = Number(this.form.get('totalPayable')?.value ?? 0);

  this.electricityPaymentHistoryService
    .getList({
      electricityBillId: this.id,
      maxResultCount: 1000
    })
    .subscribe(res => {
      const payments = res.items;
      const totalPaid = payments.reduce((sum, p) => sum + (p.paymentReceived ?? 0), 0);

      const latest = payments[0];
      this.latestPayment = latest;

      this.isBillFullyPaid = totalPaid >= payable;

      if (latest) {
        this.paymentForm.patchValue({
          transactionId: latest.transactionId,
          paymentReceived: latest.paymentReceived,
          paymentDate: this.formatDate(latest.paymentDate),
          method: latest.method
        });
      } else {
        this.paymentForm.patchValue({
          transactionId: '',
          paymentReceived: payable,
          paymentDate: this.formateDateForPayment(new Date()),
          method: 1
        });
      }

      if (this.isBillFullyPaid) {
        this.paymentForm.disable();
      } else {
        this.paymentForm.enable();
      }

      this.isPaymentModalOpen = true;
    });
}

submitPayment() {
  if (this.paymentForm.invalid || !this.id) return;

  const dto = {
    electricityBillId: this.id,
    ...this.paymentForm.value
  };

  this.electricityPaymentHistoryService.create(dto).subscribe(() => {
    this.toaster.success('::Paymentaddedsuccessfully');

    this.isPaymentModalOpen = false;

    this.loadBill(this.id!);
  });
}



}