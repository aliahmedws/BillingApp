import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumerPersonalInfoComponent } from './consumer-personal-info.component';

describe('ConsumerPersonalInfoComponent', () => {
  let component: ConsumerPersonalInfoComponent;
  let fixture: ComponentFixture<ConsumerPersonalInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConsumerPersonalInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsumerPersonalInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
