import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ElectricityBillService } from 'src/app/proxy/electricity-bills';
import { MeterInfoDto, MeterInfoLookupDto, MeterInfoService } from 'src/app/proxy/meter-infos';

@Component({
  selector: 'app-bulk-electricity-bill',
  standalone: false,
  templateUrl: './bulk-electricity-bill.component.html',
  styleUrl: './bulk-electricity-bill.component.scss'
})
export class BulkElectricityBillComponent implements OnInit{
  bulkForm: FormGroup;

  meters: MeterInfoLookupDto[] = [];
  
  selectedMeter = null;
  presentReading = null;
  previousReading = null;

  editIndex: number | null = null;

  rows: any[] = [];

  constructor(
    private meterService: MeterInfoService,
    private electricityService: ElectricityBillService,
    private fb: FormBuilder,
    private router: Router,
    private toaster: ToasterService
  ){}

  ngOnInit(): void {
    this.meterService.getMeterInfoLookup().subscribe(r => this.meters = r);

    this.bulkForm = this.fb.group({
    billingMonth: ['', Validators.required],
    meterReadingDate: ['', Validators.required],
    issueDate: ['', Validators.required],
    dueDate: ['', Validators.required],
    anyOtherCharges: [0],
    lpSurcharge: [0]
  });
  }

  loadPrevReading(meterId: string) {
    this.electricityService.getList({
      meterInfoId: meterId,
      maxResultCount: 1,
      sorting: "meterReadingDate DESC"
    }).subscribe(res => {
      this.previousReading = res.items.length ? res.items[0].presentReading : 0;
    })
  }

  addMeterRow() {
  if (!this.selectedMeter || this.presentReading == null) {
    return;
  }

  const meterId = this.selectedMeter.id;

  if(this.editIndex === null) {
    const alreadyExists = this.rows.some(r => r.meterInfoId === meterId);

    if(alreadyExists) {
      this.toaster.warn("::Thismeterisalreadyaddedinthislist.")
      return;
    }
  }

  const newRow = {
    meterInfoId: this.selectedMeter.id,
    meterNo: this.selectedMeter.meterInfo,
    previousReading: this.previousReading,
    presentReading: this.presentReading,
    units: this.presentReading - this.previousReading
  };

  if (this.editIndex !== null) {
    this.rows.splice(this.editIndex, 0, newRow);
    this.editIndex = null;
  } else {
    this.rows.push(newRow);
  }

  this.selectedMeter = null;
  this.presentReading = null;
  this.previousReading = null;
}



  generateBills() {
    const dto = {
      ...this.bulkForm.value,
      items: this.rows.map(r => ({
        meterInfoId: r.meterInfoId,
        previousReading: r.previousReading,
        presentReading: r.presentReading
      }))
    }

    this.electricityService.generateBulk(dto).subscribe(() => {
    this.toaster.success("::Bulkbillsgenerated");
    this.router.navigate(['/electricity-bills']);
  });
  }

  backToList() {
    this.router.navigate(['/electricity-bills']);
  }

  deleteRow(index: number) {
    this.rows.splice(index, 1);
  }

  editRow(index: number) {
    const row = this.rows[index];

    this.selectedMeter = this.meters.find(m => m.id === row.meterInfoId);
    this.previousReading = row.previousReading;
    this.presentReading = row.presentReading;

    this.editIndex = index;

    // remove from table temporarily (will be replaced)
    this.rows.splice(index, 1);
  }

}
