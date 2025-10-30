import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumerPersonalInfoCreateComponent } from './consumer-personal-info-create.component';

describe('ConsumerPersonalInfoCreateComponent', () => {
  let component: ConsumerPersonalInfoCreateComponent;
  let fixture: ComponentFixture<ConsumerPersonalInfoCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConsumerPersonalInfoCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsumerPersonalInfoCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
