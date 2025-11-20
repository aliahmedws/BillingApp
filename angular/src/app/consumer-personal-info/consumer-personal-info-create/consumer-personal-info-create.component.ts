import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { consumerDocumentTypeOptions, ConsumerDocumentDto, ConsumerDocumentService } from 'src/app/proxy/consumer-documents';
import {
  ConsumerPersonalInfoService,
  CreateConsumerPersonalInfoDto,
  ConsumerPersonalInfoDto,
  countryOptions,
  genderOptions,
  UpdateConsumerPersonalInfoDto,
} from 'src/app/proxy/consumer-personal-infos';
import { CustomConsumerDocumentService } from 'src/app/upload-document-services/consumer-document-service';

@Component({
  selector: 'app-consumer-personal-info-create',
  standalone: false,
  templateUrl: './consumer-personal-info-create.component.html',
  styleUrl: './consumer-personal-info-create.component.scss',
})
export class ConsumerPersonalInfoCreateComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  form: FormGroup;

  countries = countryOptions;
  genders = genderOptions;

  selectedConsumer = {} as CreateConsumerPersonalInfoDto;
  consumer = {} as ConsumerPersonalInfoDto;
  isViewMode = false;
  isEditMode = false;
  id: string | null = null;
  consumerId: string | null = null;

  currentStep = 0;

  consumerDocumentType = consumerDocumentTypeOptions;
  uploadedDocuments: ConsumerDocumentDto[] = [];
  editingInlineDocId: string | null = null;
  selectedFile: File | null = null;
  selectedDocumentType: number | null = null;
  description = '';
  issueDate: string | null = null;
  expireDate: string | null = null;

  constructor(
    private fb: FormBuilder,
    private consumerService: ConsumerPersonalInfoService,
    private consumerDocumentService: ConsumerDocumentService,
    private customConsumerDocumentService: CustomConsumerDocumentService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.queryParamMap.get('id');
    const view = this.route.snapshot.queryParamMap.get('view') === 'true';
    const edit = this.route.snapshot.queryParamMap.get('edit') === 'true';

    this.isViewMode = view;
    this.isEditMode = edit;

    this.buildForm();

    if (this.id) {
      this.consumerService.get(this.id).subscribe(data => {
        this.consumer = data;
        this.consumerId = data.id;

        const formatDob = data.dob ? data.dob.split('T')[0] : null;

        this.form.patchValue({ ...data, dob: formatDob });

        // if backend includes documents on the DTO, load them
        this.uploadedDocuments = (data as any).consumerDocuments || [];

        if (this.isViewMode) {
          this.form.disable();
        }
      });
    }
  }

  buildForm() {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const phonePattern = /^[0-9]{10,15}$/;
    const cnicPattern = /^[0-9]{13}$/;

    this.form = this.fb.group({
      firstName: [this.selectedConsumer.firstName || '', [Validators.required, Validators.maxLength(100)]],
      lastName: [this.selectedConsumer.lastName || '', [Validators.required, Validators.maxLength(100)]],
      phone: [this.selectedConsumer.phone || '', [Validators.required, Validators.pattern(phonePattern)]],
      cnic: [this.selectedConsumer.cnic || '', [Validators.required, Validators.pattern(cnicPattern)]],
      gender: [this.selectedConsumer.gender || null, Validators.required],
      dob: [this.selectedConsumer.dob || null, Validators.required],
      email: [this.selectedConsumer.email || null, Validators.pattern(emailPattern)],
      alternativePersonName: [this.selectedConsumer.alternativePersonName || '',[Validators.maxLength(100)]],
      alternativePersonPhone: [this.selectedConsumer.alternativePersonPhone || '',[Validators.pattern(phonePattern), Validators.maxLength(15)]],
      alternativePersonEmail: [this.selectedConsumer.alternativePersonEmail || '',[Validators.pattern(emailPattern), Validators.maxLength(150)]],
      alternativePersonCNIC: [this.selectedConsumer.alternativePersonCNIC || '',[Validators.pattern(cnicPattern), Validators.maxLength(13)]], 
      address: this.fb.group({
        street: [this.selectedConsumer.address?.street || '',[Validators.required, Validators.maxLength(200)]],
        city: [this.selectedConsumer.address?.city || '',[Validators.required, Validators.maxLength(100)]],
        state: [this.selectedConsumer.address?.state || '',[Validators.required, Validators.maxLength(100)]],
        country: [this.selectedConsumer.address?.country || null, Validators.required],
        postalCode: [this.selectedConsumer.address?.postalCode || '',[Validators.required, Validators.maxLength(20)]],
      }),
    });
  }

  saveAndNext() {
    if (this.form.invalid) return;

    const dto =
      this.form.value as
      | CreateConsumerPersonalInfoDto
      | UpdateConsumerPersonalInfoDto;

    if (this.id) {
      this.consumerService.update(this.id, dto).subscribe(() => {
        this.nextStep();
      });
    } else {
      this.consumerService.create(dto).subscribe(res => {
        this.id = res.id;
        this.consumerId = res.id;
        this.nextStep();
      });
    }
  }

  // ---------- Document Upload Logic (like PlotInfo) ----------

  onFileChange(event: any) {
  const file = event.target.files?.[0];
  this.selectedFile = file ?? null;
}


  upload() {
    debugger;
    if (!this.selectedFile) {
      this.toaster.warn('::Pleaseselectafilefirst');
      return;
    }

    if (!this.consumerId) {
      this.toaster.error('::ConsumerNotSavedYet');
      return;
    }

    if (!this.selectedDocumentType) {
      this.toaster.warn('::PleaseSelectDocumentType');
      return;
    }

    const Document_Type_Other = 5;
    if (this.selectedDocumentType === Document_Type_Other && !this.description) {
      this.toaster.warn('::PleaseEnterDescriptionForOtherDocumentType');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('input.consumerId', this.consumerId);
    formData.append('input.consumerDT',this.selectedDocumentType.toString());

    if (this.description) {
      formData.append('input.description', this.description);
    }
    if (this.issueDate) {
      formData.append('input.issueDate', this.issueDate);
    }
    if (this.expireDate) {
      formData.append('input.expireDate', this.expireDate);
    }

    formData.append('input.isVerified', 'false');

    this.customConsumerDocumentService.uploadFormData(formData).subscribe({
      next: (res: any) => {
        this.toaster.success('::Fileuploadedsuccessfully');
        this.uploadedDocuments.push(res);

        // reset fields
        this.selectedFile = null;
        this.description = '';
        this.issueDate = null;
        this.expireDate = null;
        this.selectedDocumentType = null;

        if (this.fileInput) {
          this.fileInput.nativeElement.value = '';
        }
      },
      error: err => {
        this.toaster.error('::Uploadfailed');
        console.error('Upload error:', err);
        console.log('Response body:', err.error);
      },
    });
  }

  uploadAndFinish() {
  if (!this.selectedFile) {
    this.toaster.success('::UpdateSuccessfully');
    this.backToList();
    return;
  }

  if (!this.consumerId) {
    this.toaster.error('::ConsumerNotSavedYet');
    return;
  }

  if (!this.selectedDocumentType) {
    this.toaster.warn('::PleaseSelectDocumentType');
    return;
  }

  const formData = new FormData();
  formData.append('file', this.selectedFile);
  formData.append('input.consumerId', this.consumerId);
  formData.append('input.consumerDT', this.selectedDocumentType.toString());

  if (this.description) {
    formData.append('input.description', this.description);
  }
  if (this.issueDate) {
    formData.append('input.issueDate', this.issueDate);
  }
  if (this.expireDate) {
    formData.append('input.expireDate', this.expireDate);
  }

  formData.append('input.isVerified', 'false');

  this.customConsumerDocumentService
    .uploadFormData(formData)
    .subscribe({
      next: () => {
        this.toaster.success('::SavedSuccessfully');
        this.backToList();
      },
      error: err => {
        this.toaster.error('::UploadFailed');
        console.error('UploadAndFinish error:', err);
        console.log('Response body:', err.error);
      },
    });
}


  deleteDocument(id: string, showConfirm: boolean = true) {
    const doDelete = () => {
      this.consumerDocumentService.delete(id).subscribe(() => {
        this.uploadedDocuments = this.uploadedDocuments.filter(
          doc => doc.id !== id
        );
        if (showConfirm) {
          this.toaster.success('::SuccessfullyDeleted');
        }
      });
    };

    if (showConfirm) {
      this.confirmation
        .warn('::AreYouSureToDelete', '::AreYouSure')
        .subscribe(status => {
          if (status === Confirmation.Status.confirm) {
            doDelete();
          }
        });
    } else {
      doDelete();
    }
  }

  saveInlineEdit(doc: ConsumerDocumentDto) {
    if (!doc.consumerDT) {
      this.toaster.warn('::PleaseCompleteTheFields');
      return;
    }

    const Document_Type_Other = 5;
    if (doc.consumerDT === Document_Type_Other && !doc.description) {
      this.toaster.warn(
        '::PleaseEnterDescriptionForOtherDocumentType'
      );
      return;
    }

    this.consumerDocumentService
      .update(doc.id, {
        consumerId: this.consumerId,
        consumerDT: doc.consumerDT,
        description: doc.description,
        issueDate: doc.issueDate,
        expireDate: doc.expireDate,
        isVerified: doc.isVerified ?? false,
      })
      .subscribe(updated => {
        const index = this.uploadedDocuments.findIndex(
          d => d.id === doc.id
        );
        if (index > -1) {
          this.uploadedDocuments[index] = updated;
        }

        this.toaster.success('::SuccessfullyUpdated');
        this.editingInlineDocId = null;
      });
  }

  nextStep() {
    this.currentStep++;
  }

  prevStep() {
    this.currentStep--;
  }

  enableEditMode() {
    this.isViewMode = false;
    this.isEditMode = true;
    this.form.enable();
  }

  backToList() {
    this.router.navigate(['/consumerPersonalInfos']);
  }
}