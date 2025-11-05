import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMeterInfoComponent } from './create-meter-info.component';

describe('CreateMeterInfoComponent', () => {
  let component: CreateMeterInfoComponent;
  let fixture: ComponentFixture<CreateMeterInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateMeterInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateMeterInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
