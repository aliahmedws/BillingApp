import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-consumer-document',
  standalone: false,
  templateUrl: './create-consumer-document.component.html',
  styleUrl: './create-consumer-document.component.scss'
})
export class CreateConsumerDocumentComponent implements OnInit {
  form: FormGroup;
  documents: any[] = [];
  isCnicSelected = false;

  consumers = [
    { id: '1', name: 'Ismail Ali' },
    { id: '2', name: 'Rafaquat Kayani' },
  ];

  documentTypes = [
    { value: 'CNIC', label: 'CNIC' },
    { value: 'Passport', label: 'Passport' },
    { value: 'DrivingLicense', label: 'Driving License' },
  ];

  selectedFiles: { [key: string]: File | null } = { front: null, back: null, single: null };

  constructor(private fb: FormBuilder, private toaster: ToasterService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      consumerId: [null, Validators.required],
      documentType: [null, Validators.required],
      issueDate: [null],
      expiryDate: [null],
      description: [''],
      isVerified: [false],
      verificationData: [''],
      verifiedDate: [null],
    });
  }

  onDocumentTypeChange(value: string) {
    this.isCnicSelected = value === 'CNIC';
    this.selectedFiles = { front: null, back: null, single: null };
  }

  onFileSelect(event: any, type: 'front' | 'back' | 'single') {
    const file = event.target.files?.[0];
    if (file) {
      this.selectedFiles[type] = file;
    }
  }

  addDocument() {
    if (this.form.invalid) return;

    const formValue = this.form.value;
    const consumer = this.consumers.find(c => c.id === formValue.consumerId);
    const docType = this.documentTypes.find(d => d.value === formValue.documentType);

    const files: any[] = [];
    if (this.isCnicSelected) {
      if (this.selectedFiles.front)
        files.push({ name: 'Front', preview: URL.createObjectURL(this.selectedFiles.front) });
      if (this.selectedFiles.back)
        files.push({ name: 'Back', preview: URL.createObjectURL(this.selectedFiles.back) });
    } else if (this.selectedFiles.single) {
      files.push({ name: this.selectedFiles.single.name, preview: URL.createObjectURL(this.selectedFiles.single) });
    }

    this.documents.push({
      ...formValue,
      consumerName: consumer?.name,
      documentTypeLabel: docType?.label,
      files,
    });

    this.form.reset({ isVerified: false });
    this.selectedFiles = { front: null, back: null, single: null };
    this.isCnicSelected = false;
    this.toaster.success('Document added successfully');
  }

  removeDocument(index: number) {
    this.documents.splice(index, 1);
  }

  submitAll() {
    console.log('Documents to submit:', this.documents);
    this.toaster.success('All documents submitted successfully');
    // Here you can implement API upload logic
  }
}