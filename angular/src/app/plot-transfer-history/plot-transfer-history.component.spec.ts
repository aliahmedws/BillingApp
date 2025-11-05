import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlotTransferHistoryComponent } from './plot-transfer-history.component';

describe('PlotTransferHistoryComponent', () => {
  let component: PlotTransferHistoryComponent;
  let fixture: ComponentFixture<PlotTransferHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlotTransferHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlotTransferHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
