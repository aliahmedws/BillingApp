import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateConsumerDocumentComponent } from './create-consumer-document.component';

describe('CreateConsumerDocumentComponent', () => {
  let component: CreateConsumerDocumentComponent;
  let fixture: ComponentFixture<CreateConsumerDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateConsumerDocumentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateConsumerDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
