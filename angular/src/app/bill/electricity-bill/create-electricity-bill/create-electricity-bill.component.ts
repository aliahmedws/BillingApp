import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { catchError, forkJoin, map, of, switchMap, take } from 'rxjs';
import { ElectricityBillTemplateService } from 'src/app/proxy/electricity-bill-templates';
import {
  ElectricityBillService,
  CreateElectricityBillDto,
  ElectricityBillDto,
} from 'src/app/proxy/electricity-bills';
import {
  ElectricityPaymentHistoryDto,
  ElectricityPaymentHistoryService,
} from 'src/app/proxy/electricity-payment-histories';
import { GovtChargeService } from 'src/app/proxy/govt-charges';
import { IescoChargeService } from 'src/app/proxy/iesco-charges';
import { billStatusOptions } from 'src/app/proxy/maintenance-bills';
import { MeterInfoLookupDto, MeterInfoService } from 'src/app/proxy/meter-infos';
import { SocietyChargeService } from 'src/app/proxy/society-charges';

@Component({
  selector: 'app-create-electricity-bill',
  standalone: false,
  templateUrl: './create-electricity-bill.component.html',
  styleUrl: './create-electricity-bill.component.scss',
})
export class CreateElectricityBillComponent implements OnInit {
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
    private electricityBillTemplateService: ElectricityBillTemplateService,
    private societyChargeService: SocietyChargeService,
    private governmentService: GovtChargeService,
    private iescoService: IescoChargeService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.mode = (this.route.snapshot.queryParamMap.get('mode') as any) ?? 'create';

    this.buildForm();
    this.buildPaymentForm();
    this.loadStaticCharges();
    this.loadMeters();

    if(this.mode === 'view')  this.applyEditability();

    if (this.mode !== 'create' && this.id) {
      this.loadBill(this.id);
    }

    if (this.mode === 'view') this.form.disable();

    this.setupAutoCalculation();

    this.form.get('meterInfoId')?.valueChanges.subscribe(meterId => {
      if (!meterId) return;

      if (this.mode === 'create') {
        this.loadLatestMeterReading(meterId);
      }

      this.loadSocietyChargesForMeter(meterId);
    });
  }

  buildForm() {
    this.form = this.fb.group({
      meterInfoId: [this.selectedElectricityBill.meterInfoId || null, Validators.required],

      previousReading: [{ value: this.selectedElectricityBill.previousReading || 0, disabled: true }],
      presentReading: [this.selectedElectricityBill.presentReading || 0, Validators.required],
      unitsConsumed: [{ value: this.selectedElectricityBill.consumedUnits || 0, disabled: true }],

      meterReadingDate: [
        this.selectedElectricityBill.meterReadingDate || null,
        Validators.required,
      ],
      billingMonth: [this.selectedElectricityBill.billingMonth || null, Validators.required],
      issueDate: [this.selectedElectricityBill.issueDate || null, Validators.required],
      dueDate: [this.selectedElectricityBill.dueDate || null, Validators.required],

      currentMonthBill: [
        { value: this.selectedElectricityBill.currentMonthBill || 0, disabled: true },
      ],
      billAdjustment: [this.selectedElectricityBill.billAdjustment || 0],
      anyOtherCharges: [this.selectedElectricityBill.anyOtherCharges || 0],
      lpSurcharge: [{ value: this.selectedElectricityBill.lpSurcharge || 0, disabled: true }],

      //readonly
      totalGovernmentCharges: [{ value: 0, disabled: true }],
      totalIESCOCharges: [{ value: 0, disabled: true }],
      totalSocietyCharges: [{ value: 0, disabled: true }],  

      payableDueDateAmount: [{ value: this.selectedElectricityBill.payableDueDateAmount || 0, disabled: true }],
      payableAfterDueDateAmount: [{ value: this.selectedElectricityBill.payableAfterDueDateAmount || 0, disabled: true }],
      
      status: [this.selectedElectricityBill.status ?? this.billStatus[0].value],
      arrears: [this.selectedElectricityBill.arrears || 0, Validators.required],
    });

     this.applyEditability(); 
  }

  buildPaymentForm() {
    this.paymentForm = this.fb.group({
      transactionId: [
        this.selectedElectricityBillPaymentHistory.transactionId || '',
        Validators.required,
      ],
      paymentReceived: [
        this.selectedElectricityBillPaymentHistory.paymentReceived || 0,
        Validators.required,
      ],
      paymentDate: [
        this.selectedElectricityBillPaymentHistory.paymentDate || '',
        Validators.required,
      ],
      method: [this.selectedElectricityBillPaymentHistory.method || 1, Validators.required],
    });
  }

  private applyEditability(): void {
  const editable = new Set([
    'meterInfoId',
    'presentReading',
    'meterReadingDate',
    'billingMonth',
    'issueDate',
    'dueDate',
    'billAdjustment',
    'anyOtherCharges',
    'arrears',
    'status',
  ]);

  Object.keys(this.form.controls).forEach(name => {
    const ctrl = this.form.get(name);
    if (!ctrl) return;

    if (this.mode === 'view') {
      ctrl.disable({ emitEvent: false });
      return;
    }

    if (editable.has(name)) ctrl.enable({ emitEvent: false });
    else ctrl.disable({ emitEvent: false }); 
  });
}


  loadStaticCharges() {
    forkJoin({
      govt: this.governmentService.getTotalCharges(),
      iesco: this.iescoService.getTotalIescoCharges(),
    }).subscribe(({ govt, iesco }) => {
      this.form.patchValue(
        {
          totalGovernmentCharges: govt ?? 0,
          totalIESCOCharges: iesco ?? 0,
        },
        {
          emitEvent: false,
        }
      );
      this.recalculateTotal();
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
        dueDate: this.formatDate(res.dueDate),
      };

      this.form.patchValue(formatted, { emitEvent: false });
      this.applyEditability();

      if (formatted.meterInfoId) {
        this.loadSocietyChargesForMeter(formatted.meterInfoId);
      }
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
      queryParams: { id: this.id, mode: 'edit' },
    });

    this.mode = 'edit';
    this.applyEditability();
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

  round2(n: number) {
    return Math.round((n + Number.EPSILON) * 100) / 100;
  }

  isAfterDueDate(paymentDate: Date): boolean {
    const dueStr = this.form.get('dueDate')?.value;
    if (!dueStr) return false;

    const dueDate = new Date(dueStr);
    const pay = new Date(paymentDate);

    dueDate.setHours(0, 0, 0, 0);
    pay.setHours(0, 0, 0, 0);

    return pay > dueDate;
  }

recalculateTotal(): void {
  const val = (name: string) => Number(this.form.get(name)?.value ?? 0);

  const baseDue =
    val('currentMonthBill') +
    val('totalGovernmentCharges') +
    val('totalIESCOCharges') +
    val('totalSocietyCharges') +
    val('anyOtherCharges') +
    val('arrears') -
    val('billAdjustment');

  const payableDue = Math.max(0, this.round2(baseDue));
  const lp = this.round2(payableDue * 0.10);
  const payableAfter = this.round2(payableDue + lp);

  this.form.patchValue(
    {
      payableDueDateAmount: payableDue,
      lpSurcharge: lp,
      payableAfterDueDateAmount: payableAfter,
    },
    { emitEvent: false }
  );
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
      queryParams: { id: this.id },
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
    });
  }

  loadLatestMeterReading(meterId: string) {
    this.electricityService
      .getList({
        meterInfoId: meterId,
        maxResultCount: 1,
        sorting: 'meterReadingDate DESC',
      })
      .subscribe(result => {
        const lastBill = result.items[0];

        if (lastBill) {
          this.form.patchValue({
            previousReading: lastBill.presentReading,
          });
        } else {
          this.form.patchValue({
            previousReading: 0,
          });
        }
      });
  }


openPaymentModal() {
  if (!this.id) return;

  this.electricityPaymentHistoryService
    .getList({
      electricityBillId: this.id,
      maxResultCount: 1000,
    })
    .subscribe(res => {
      const payments = (res.items ?? []).slice();

      payments.sort((a: any, b: any) =>
        new Date(b.paymentDate).getTime() - new Date(a.paymentDate).getTime()
      );

      const totalPaid = this.round2(
        payments.reduce((sum, p) => sum + (Number(p.paymentReceived) || 0), 0)
      );

      const today = new Date();
      const payableDue = Number(this.form.get('payableDueDateAmount')?.value ?? 0);
      const payableAfter = Number(this.form.get('payableAfterDueDateAmount')?.value ?? 0);

      const targetTotal = this.isAfterDueDate(today) ? payableAfter : payableDue;

      const remaining = this.round2(Math.max(0, targetTotal - totalPaid));
      this.isBillFullyPaid = remaining <= 0;

      this.paymentForm.reset(
        {
          transactionId: '',
          paymentReceived: remaining,
          paymentDate: this.formateDateForPayment(today),
          method: payments[0]?.method ?? 1,
        },
        { emitEvent: false }
      );

      if (this.isBillFullyPaid) this.paymentForm.disable({ emitEvent: false });
      else this.paymentForm.enable({ emitEvent: false });

      this.isPaymentModalOpen = true;
    });
}


  submitPayment() {
    if (this.paymentForm.invalid || !this.id) return;

    const dto = {
      electricityBillId: this.id,
      ...this.paymentForm.value,
    };

    this.electricityPaymentHistoryService.create(dto).subscribe(() => {
      this.toaster.success('::Paymentaddedsuccessfully');

      this.isPaymentModalOpen = false;

      this.loadBill(this.id!);
    });
  }

  loadSocietyChargesForMeter(meterId: string): void {
  this.meterService
    .get(meterId)
    .pipe(
      take(1),
      map((meter: any) => (meter?.plotSizeName ?? meter?.plot?.plotSize?.plotSizeName ?? '').trim()),
      switchMap((plotSizeName: string) => {
        if (!plotSizeName) return of(0);

        return this.societyChargeService
          .getTotalChargesByPlotSizeNameByPlotSize(plotSizeName)
          .pipe(
            catchError(err => {
              console.error('Society charges API failed', err);
              return of(0);
            })
          );
      }),
      catchError(err => {
        console.error('Meter API failed', err);
        return of(0);
      })
    )
    .subscribe(total => {
      this.form.patchValue({ totalSocietyCharges: total ?? 0 }, { emitEvent: false });
      this.recalculateTotal();
    });
}


  printBill() {
    if (!this.id) {
      this.toaster.warn('::Nobillavailabletoprint');
      return;
    }

    this.electricityBillTemplateService.getPrintHtml(this.id).subscribe({
      next: (file: Blob) => {
        const blob = new Blob([file], { type: 'text/html' });
        const url = URL.createObjectURL(blob);

        const printWindow = window.open(url, '_blank');

        if (!printWindow) {
          this.toaster.error('::Unabletoopenprintwindow');
          return;
        }

        printWindow.onload = () => {
          printWindow.print();
        };
      },
      error: () => {
        this.toaster.error('::Errorgeneratingprintfile.');
      },
    });
  }
}
