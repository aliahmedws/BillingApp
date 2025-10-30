import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GovtchargeComponent } from './govtcharge.component';

describe('GovtchargeComponent', () => {
  let component: GovtchargeComponent;
  let fixture: ComponentFixture<GovtchargeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GovtchargeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GovtchargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
