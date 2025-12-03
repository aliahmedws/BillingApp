import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MaintenanceBillDto, MaintenanceBillService } from 'src/app/proxy/maintenance-bills';

@Component({
  selector: 'app-print-maintenance-bill',
  standalone: false,
  templateUrl: './print-maintenance-bill.component.html',
  styleUrl: './print-maintenance-bill.component.scss'
})
export class PrintMaintenanceBillComponent implements OnInit{
 bill: MaintenanceBillDto;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly maintenanceBill: MaintenanceBillService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.queryParamMap.get('id');

    if (id) {
      this.maintenanceBill.get(id).subscribe(res => {
        this.bill = res;

        // Delay printing until content is fully rendered
        setTimeout(() => {
          window.print();
        }, 500);
      });
    }
  }
}
