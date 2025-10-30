import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumerDocumentComponent } from './consumer-document.component';

describe('ConsumerDocumentComponent', () => {
  let component: ConsumerDocumentComponent;
  let fixture: ComponentFixture<ConsumerDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConsumerDocumentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsumerDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
