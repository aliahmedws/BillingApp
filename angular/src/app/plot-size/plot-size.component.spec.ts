import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlotSizeComponent } from './plot-size.component';

describe('PlotSizeComponent', () => {
  let component: PlotSizeComponent;
  let fixture: ComponentFixture<PlotSizeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlotSizeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlotSizeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
