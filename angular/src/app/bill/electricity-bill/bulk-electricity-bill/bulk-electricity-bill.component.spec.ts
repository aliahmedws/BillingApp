import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BulkElectricityBillComponent } from './bulk-electricity-bill.component';

describe('BulkElectricityBillComponent', () => {
  let component: BulkElectricityBillComponent;
  let fixture: ComponentFixture<BulkElectricityBillComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BulkElectricityBillComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BulkElectricityBillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
