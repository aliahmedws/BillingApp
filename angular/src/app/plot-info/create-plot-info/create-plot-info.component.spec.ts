import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePlotInfoComponent } from './create-plot-info.component';

describe('CreatePlotInfoComponent', () => {
  let component: CreatePlotInfoComponent;
  let fixture: ComponentFixture<CreatePlotInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreatePlotInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePlotInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
