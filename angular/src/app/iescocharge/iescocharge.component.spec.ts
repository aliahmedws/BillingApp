import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IescochargeComponent } from './iescocharge.component';

describe('IescochargeComponent', () => {
  let component: IescochargeComponent;
  let fixture: ComponentFixture<IescochargeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IescochargeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IescochargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
