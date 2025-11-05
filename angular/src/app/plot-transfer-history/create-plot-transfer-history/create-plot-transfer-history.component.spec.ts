import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePlotTransferHistoryComponent } from './create-plot-transfer-history.component';

describe('CreatePlotTransferHistoryComponent', () => {
  let component: CreatePlotTransferHistoryComponent;
  let fixture: ComponentFixture<CreatePlotTransferHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreatePlotTransferHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePlotTransferHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
