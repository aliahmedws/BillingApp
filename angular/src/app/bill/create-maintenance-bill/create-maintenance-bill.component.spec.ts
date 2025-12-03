import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMaintenanceBillComponent } from './create-maintenance-bill.component';

describe('CreateMaintenanceBillComponent', () => {
  let component: CreateMaintenanceBillComponent;
  let fixture: ComponentFixture<CreateMaintenanceBillComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateMaintenanceBillComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateMaintenanceBillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
