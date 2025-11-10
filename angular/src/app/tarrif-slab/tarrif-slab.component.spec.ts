import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TarrifSlabComponent } from './tarrif-slab.component';

describe('TarrifSlabComponent', () => {
  let component: TarrifSlabComponent;
  let fixture: ComponentFixture<TarrifSlabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TarrifSlabComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TarrifSlabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
