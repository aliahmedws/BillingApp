import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SocietychargeComponent } from './societycharge.component';

describe('SocietychargeComponent', () => {
  let component: SocietychargeComponent;
  let fixture: ComponentFixture<SocietychargeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SocietychargeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SocietychargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
