import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import {
  ConsumerPersonalInfoLookupDto,
  ConsumerPersonalInfoService
} from 'src/app/proxy/consumer-personal-infos';

import {
  billStatusOptions,
  CreateMaintenanceBillDto,
  MaintenanceBillDto,
  MaintenanceBillService
} from 'src/app/proxy/maintenance-bills';

import { PlotInfoLookupDto, PlotInfoService } from 'src/app/proxy/plot-infos';
import { SocietyChargeService } from 'src/app/proxy/society-charges';

@Component({
  selector: 'app-create-maintenance-bill',
  standalone: false,
  templateUrl: './create-maintenance-bill.component.html',
  styleUrl: './create-maintenance-bill.component.scss'
})
export class CreateMaintenanceBillComponent implements OnInit {
  
  form!: FormGroup;

  mode: 'create' | 'edit' | 'view' = 'create';
  id: string | null = null;

  selectedMaintenanceBill = {} as MaintenanceBillDto;
  consumers: ConsumerPersonalInfoLookupDto[] = [];
  plots: PlotInfoLookupDto[] = [];

  billStatus = billStatusOptions;

  hasSocietyChargesForSelectedPlot = false;

  chargeLabels: Record<string, string> = {
    waterCharges: 'WaterCharges',
    securityCharges: 'SecurityCharges',
    arrears: 'Arrears',
    otherCharges: 'OtherCharges',
    refundOrBenefit: 'RefundOrBenefit',
    anyOtherWorkCharges: 'AnyOtherWorkCharges',
    latePaymentSurcharge: 'LatePaymentSurcharge',
  };

  chargeFields: string[] = [
    'waterCharges',
    'securityCharges',
    'arrears',
    'otherCharges',
    'refundOrBenefit',
    'anyOtherWorkCharges',
    'latePaymentSurcharge'
  ];

  constructor(
    private fb: FormBuilder,
    private consumerService: ConsumerPersonalInfoService,
    private plotService: PlotInfoService,
    private maintenanceBillService: MaintenanceBillService,
    private societyService: SocietyChargeService,
    private router: Router,
    private toaster: ToasterService,
    private route: ActivatedRoute
  ) {}

  // ---------------- INIT ----------------
  ngOnInit(): void {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.mode = (this.route.snapshot.queryParamMap.get('mode') as any) ?? 'create';

    this.buildForm();
    this.loadLookups();

    if (this.mode !== 'create' && this.id) {
      this.loadBill(this.id);
    }

    if (this.mode === 'view') {
      this.form.disable();
    }

    this.setupTotalsRecalculation();
    this.handleConsumerChange();
    this.handlePlotChange();
    this.setupPartialBillingCalculation();
  }

  // ---------------- FORM BUILDING ----------------
  buildForm() {
    this.form = this.fb.group({
      consumerId: [this.selectedMaintenanceBill.consumerId || null, Validators.required],
      plotInfoId: [this.selectedMaintenanceBill.plotInfoId || null, Validators.required],

      billingMonth: [this.selectedMaintenanceBill.billingMonth || null, Validators.required],
      issueDate: [this.selectedMaintenanceBill.issueDate || null, Validators.required],
      dueDate: [this.selectedMaintenanceBill.dueDate || null, Validators.required],

      waterCharges: [{ value: this.selectedMaintenanceBill.waterCharges || 0, disabled: true }],
      securityCharges: [{ value: this.selectedMaintenanceBill.securityCharges || 0, disabled: true }],
      currentBill: [{ value: this.selectedMaintenanceBill.currentBill || 0, disabled: true }],
      arrears: [{ value: this.selectedMaintenanceBill.arrears || 0, disabled: true }],
      otherCharges: [{ value: this.selectedMaintenanceBill.otherCharges || 0, disabled: true }],

      refundOrBenefit: [this.selectedMaintenanceBill.refundOrBenefit || 0],
      anyOtherWorkCharges: [this.selectedMaintenanceBill.anyOtherWorkCharges || 0],
      latePaymentSurcharge: [this.selectedMaintenanceBill.latePaymentSurcharge || 0],

      partialMonths: [{ value: this.selectedMaintenanceBill.partialMonths || null, disabled: true }],
      partialMonthlyAmount: [{ value: this.selectedMaintenanceBill.partialMonthlyAmount || null, disabled: true }],

      paymentBeforeDueDate: [{ value: this.selectedMaintenanceBill.paymentBeforeDueDate || 0, disabled: true }],
      payableAfterDueDate: [{ value: this.selectedMaintenanceBill.payableAfterDueDate || 0, disabled: true }],

      status: [this.selectedMaintenanceBill.status ?? 1]
    });
  }

  // ---------------- LOAD BILL ----------------
  loadBill(id: string) {
    this.maintenanceBillService.get(id).subscribe(res => {
      this.selectedMaintenanceBill = res;
      this.buildForm();

      if (this.mode === 'view') {
        this.form.disable();
      }

      this.plotService.getPlotInfoByConsumerId(res.consumerId).subscribe(plotList => {
        this.plots = plotList;
        this.form.get('plotInfoId')?.setValue(res.plotInfoId, { emitEvent: false });
      });

      this.recalculateTotals();
    });
  }

  loadLookups() {
    this.consumerService.consumerPersonalInfoLookup().subscribe(res => (this.consumers = res));
    this.plotService.getPlotLookUp().subscribe(res => (this.plots = res));
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const dto = this.form.getRawValue() as CreateMaintenanceBillDto;

    if (this.mode === 'edit' && this.id) {
      this.maintenanceBillService.update(this.id, dto).subscribe(() => {
        this.toaster.success('Updated');
        this.backToList();
      });
    } else {
      this.maintenanceBillService.create(dto).subscribe(() => {
        this.toaster.success('Created');
        this.backToList();
      });
    }
  }

  enableEdit() {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { id: this.id, mode: 'edit' }
    });

    this.mode = 'edit';
    this.form.enable();
  }

  setupTotalsRecalculation() {
    this.form.valueChanges.subscribe(() => this.recalculateTotals());
  }

  recalculateTotals() {
    const val = (f: string) => Number(this.form.get(f)?.value ?? 0);

    const current =
      val('waterCharges') +
      val('securityCharges') +
      val('arrears') +
      val('otherCharges') +
      val('anyOtherWorkCharges') -
      val('refundOrBenefit');

    this.form.get('currentBill')?.setValue(current, { emitEvent: false });
    this.form.get('paymentBeforeDueDate')?.setValue(current, { emitEvent: false });
    this.form.get('payableAfterDueDate')?.setValue(current + val('latePaymentSurcharge'), {
      emitEvent: false
    });
  }

  handleConsumerChange() {
    const plot = this.form.get('plotInfoId');

    this.form.get('consumerId')?.valueChanges.subscribe(consumerId => {
      if (this.mode !== 'view') plot?.enable({ emitEvent: false });

      if (!consumerId) return;

      this.plotService.getPlotInfoByConsumerId(consumerId).subscribe(res => {
        this.plots = res;
      });
    });
  }

  handlePlotChange() {
    this.form.get('plotInfoId')?.valueChanges.subscribe(plotId => {
      if (!plotId) {
        this.resetAutoCharges();
        return;
      }

      this.hasSocietyChargesForSelectedPlot = true;

      const plot = this.plots.find(p => p.id === plotId);
      if (!plot?.plotSize) {
        this.resetAutoCharges();
        return;
      }

      this.societyService.getByPlotSizeName(plot.plotSize).subscribe(res => {
        if (!res) {
          this.resetAutoCharges();
          this.toaster.warn('::NoSocietyChargesConfiguredForThisPlotSize');
          return;
        }

        this.form.patchValue(
          {
            waterCharges: res.waterCharges ?? 0,
            securityCharges: res.securityCharges ?? 0,
            otherCharges: res.otherCharges ?? 0
          },
          { emitEvent: true }
        );
      });
    });
  }

  resetAutoCharges() {
    this.form.patchValue(
      {
        waterCharges: 0,
        securityCharges: 0,
        otherCharges: 0,
        arrears: 0,
        refundOrBenefit: 0,
        anyOtherWorkCharges: 0
      },
      { emitEvent: true }
    );

    this.hasSocietyChargesForSelectedPlot = false;
  }

private setupPartialBillingCalculation() {

  this.form.get('status')?.valueChanges.subscribe(status => {

    const months = this.form.get('partialMonths');
    const amount = this.form.get('partialMonthlyAmount');

    if (status === 2) { // 2 = PartiallyPaid
      months?.enable({ emitEvent: false });
      amount?.enable({ emitEvent: false });
    } else {
      months?.disable({ emitEvent: false });
      amount?.disable({ emitEvent: false });
      months?.setValue(null, { emitEvent: false });
      amount?.setValue(null, { emitEvent: false });
    }
  });

  this.form.get('partialMonths')?.valueChanges.subscribe(months => {

    if (!months || months <= 0) {
      this.form.get('partialMonthlyAmount')?.setValue(null, { emitEvent: false });
      return;
    }

    const total = Number(this.form.get('paymentBeforeDueDate')?.value ?? 0);

    const monthly = +(total / months).toFixed(2);

    this.form.get('partialMonthlyAmount')?.setValue(monthly, { emitEvent: false });
  });
}


  backToList() {
    this.router.navigate(['/Bills']);
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

    this.router.navigate(['/print-maintenance-bills'], {
      queryParams: { id: this.id }
    });
  }
}
