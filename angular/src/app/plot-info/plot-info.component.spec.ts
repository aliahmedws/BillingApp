import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlotInfoComponent } from './plot-info.component';

describe('PlotInfoComponent', () => {
  let component: PlotInfoComponent;
  let fixture: ComponentFixture<PlotInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlotInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlotInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
