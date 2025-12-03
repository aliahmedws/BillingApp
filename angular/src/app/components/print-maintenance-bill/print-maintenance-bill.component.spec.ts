import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintMaintenanceBillComponent } from './print-maintenance-bill.component';

describe('PrintMaintenanceBillComponent', () => {
  let component: PrintMaintenanceBillComponent;
  let fixture: ComponentFixture<PrintMaintenanceBillComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PrintMaintenanceBillComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrintMaintenanceBillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
