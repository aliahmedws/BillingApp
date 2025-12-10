import { ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import {
  meterDocumentTypeOptions,
  OwnMeterDocumentService,
} from 'src/custom-services/upload-document-services/meter-document-service';
import { BlockLookupDto, BlockService } from 'src/app/proxy/blocks';
import {
  ConsumerPersonalInfoLookupDto,
  ConsumerPersonalInfoService,
} from 'src/app/proxy/consumer-personal-infos';
import { MeterDocumentService } from 'src/app/proxy/meter-document.service';
import { MeterDocumentDto } from 'src/app/proxy/meter-documents';
import {
  MeterInfoDto,
  meterTypeOptions,
  meterStatusOptions,
  MeterInfoService,
  MeterStatus,
  CreateMeterInfoDto,
  UpdateMeterInfoDto,
  meterCategoryOptions,
} from 'src/app/proxy/meter-infos';
import { PhaseLookUp, PhaseService } from 'src/app/proxy/phases';
import { PlotInfoLookupDto, PlotInfoService } from 'src/app/proxy/plot-infos';

@Component({
  selector: 'app-create-meter-info',
  standalone: false,
  templateUrl: './create-meter-info.component.html',
  styleUrl: './create-meter-info.component.scss',
  providers: [ListService],
})
export class CreateMeterInfoComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  form: FormGroup;
  isViewMode = false;
  id: string | null = null;
  meter = {} as MeterInfoDto;
  selectedMeterInfo = {} as MeterInfoDto;
  meterTypes = meterTypeOptions;
  meterCategory = meterCategoryOptions;
  meterStatuses = meterStatusOptions;
  phases = [] as PhaseLookUp[];
  plots = [] as PlotInfoLookupDto[];
  blocks = [] as BlockLookupDto[];
  consumers = [] as ConsumerPersonalInfoLookupDto[];
  uploadedDocuments: MeterDocumentDto[] = [];
  editingDocument: MeterDocumentDto | null = null;
  isEditMode = false;
  selectedFile: File | null = null;
  meterId: string = null;
  selectedDocumentType: number | null = null;
  documentTypeOptions = meterDocumentTypeOptions;
  description = '';
  editingInlineDocId: string | null = null;
  currentStep = 0;

  constructor(
    private fb: FormBuilder,
    private meterService: MeterInfoService,
    private phaseService: PhaseService,
    private blockService: BlockService,
    private plotService: PlotInfoService,
    private consumerService: ConsumerPersonalInfoService,
    private meterDocumentService: OwnMeterDocumentService,
    private meterDocService: MeterDocumentService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.isViewMode = this.route.snapshot.queryParamMap.get('view') === 'true';

    this.meterId = this.id;
    this.buildForm();

    if (this.id) {
      this.meterService.get(this.id).subscribe(data => {
        this.meter = data;

        const formatInstallationDate = data.installationDate
          ? data.installationDate.split('T')[0]
          : null;

        this.form.patchValue({ ...data, installationDate: formatInstallationDate });
        if (this.isViewMode) this.form.disable();

        this.uploadedDocuments = data.meterDocuments || [];

        if (data.phaseId) {
          this.blockService.getBlocksByPhaseId(data.phaseId).subscribe(res => {
            this.blocks = res || [];

            if (data.blockId) {
              this.plotService.getPlotsByBlockId(data.blockId).subscribe(res2 => {
                this.plots = res2 || [];
              });
            }
          });
        }
      });
    }

    this.getPhases();
    this.getPlots();
    this.getConsumers();
  }

  getPhases() {
    this.phaseService.getPhaseLookUp().subscribe(res => (this.phases = res));
  }

  getBlocks() {
    this.blockService.getBlockLookup().subscribe(res => (this.blocks = res));
  }

  getPlots() {
    this.plotService.getPlotLookUp().subscribe(res => (this.plots = res));
  }

  getConsumers() {
    this.consumerService.consumerPersonalInfoLookup().subscribe(res => (this.consumers = res));
  }

  buildForm() {
    this.form = this.fb.group({
      meterNo: [
        this.selectedMeterInfo.meterNo || '',
        [Validators.required, Validators.maxLength(64)],
      ],
      meterType: [this.selectedMeterInfo.meterType || null, Validators.required],
      meterCategory: [this.selectedMeterInfo.meterCategory || null, Validators.required],
      meterStatus: [this.selectedMeterInfo.meterStatus === MeterStatus.Active, Validators.required],
      installationDate: [this.selectedMeterInfo.installationDate || null, Validators.required],
      initialReading: [
        this.selectedMeterInfo.initialReading === 0 || 0,
        [Validators.required, Validators.min(0)],
      ],
      phaseId: [this.selectedMeterInfo.phaseId || null, Validators.required],
      blockId: [this.selectedMeterInfo.blockId || null, Validators.required],
      plotId: [this.selectedMeterInfo.plotId || null, Validators.required],
      meterOwnerId: [this.selectedMeterInfo.meterOwnerId || null, Validators.required],
      remarks: [this.selectedMeterInfo.remarks || '', Validators.maxLength(512)],
    });
  }

  saveAndNext() {
    if (this.form.invalid) return;

    const dto = this.form.value as CreateMeterInfoDto | UpdateMeterInfoDto;

    if (this.id) {
      this.meterService.update(this.id, dto).subscribe(() => {
        this.currentStep = 1;
      });
    } else if (this.meterId) {
      this.meterService.update(this.meterId, dto).subscribe(() => {
        this.currentStep = 1;
      });
    } else {
      this.meterService.create(dto).subscribe(res => {
        this.meterId = res.id;
        this.currentStep = 1;
      });
    }
  }

  uploadAndFinish() {
    if (!this.selectedFile) {
      this.toaster.success('::UpdateSuccessfully');
      this.backToList();
      return;
    }

    if (!this.selectedDocumentType) {
      this.toaster.warn('::PleaseSelectDocumentType');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('input.meterId', this.meterId);
    formData.append('input.type', this.selectedDocumentType.toString());
    if (this.description) {
      formData.append('input.description', this.description);
    }

    this.meterDocumentService.uploadFormData(formData).subscribe({
      next: () => {
        this.toaster.success('::SavedSuccessfully');
        this.backToList();
      },
      error: err => {
        this.toaster.error('::UploadFailed');
        console.error(err);
      },
    });
  }

  backToList() {
      this.router.navigate(['/meterInfos']);
  }

  onPhaseChange(phaseId: string) {
    this.form.patchValue({ blockId: null, plotId: null });
    this.blocks = [];
    this.plots = [];

    if (!phaseId) return;

    this.blockService.getBlocksByPhaseId(phaseId).subscribe(res => {
      this.blocks = res ?? [];
    });
  }

  onBlockChange(blockId: string) {
    this.form.patchValue({ plotId: null });
    this.plots = [];

    if (!blockId) return;

    this.plotService.getPlotsByBlockId(blockId).subscribe(res => {
      this.plots = res ?? [];
    });
  }

  get selectedPhaseId(): string | null {
    return this.form.get('phaseId')?.value;
  }

  get selectedBlockId(): string | null {
    return this.form.get('blockId')?.value;
  }

  deleteDocument(id: string, showConfirm: boolean = true) {
    const doDelete = () => {
      this.meterDocService.delete(id).subscribe(() => {
        this.uploadedDocuments = this.uploadedDocuments.filter(doc => doc.id !== id);
        if (showConfirm) this.toaster.success('::SuccessfullyDeleted');
      });
    };

    if (showConfirm) {
      this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          doDelete();
        }
      });
    } else {
      doDelete();
    }
  }

 onFileSelected(file: File | null) {
  this.selectedFile = file;
}

  upload() {
    if (!this.selectedFile) {
      this.toaster.warn('::Pleaseselectafilefirst');
      return;
    }

    if (!this.selectedDocumentType) {
      this.toaster.warn('::PleaseSelectDocumentType');
      return;
    }

    const Document_Type_Other = 5;
    if (this.selectedDocumentType === Document_Type_Other && !this.description?.trim()) {
      this.toaster.warn('::PleaseEnterDescriptionForOtherDocumentType');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('input.meterId', this.meterId);
    formData.append('input.type', this.selectedDocumentType.toString());
    if (this.description) {
      formData.append('input.description', this.description);
    }

    this.meterDocumentService.uploadFormData(formData).subscribe({
      next: res => {
        this.toaster.success('::Fileuploadedsuccessfully');
        this.uploadedDocuments.push(res);
        this.selectedFile = null;
        this.description = '';
        this.selectedDocumentType = null;
        this.fileInput.nativeElement.value = '';
      },
      error: err => {
        this.toaster.error('::Uploadfailed');
        console.error(err);
      },
    });
  }

  saveInlineEdit(doc: MeterDocumentDto) {
    if (!doc.meterDocumentType) {
      this.toaster.warn('::PleaseCompleteTheFields');
      return;
    }

    const Document_Type_Other = 12;
    if (doc.meterDocumentType === Document_Type_Other && !doc.description?.trim()) {
      this.toaster.warn('::PleaseEnterDescriptionForOtherDocumentType');
      return;
    }

    this.meterDocService
      .update(doc.id, {
        type: doc.meterDocumentType,
        description: doc.description,
        meterId: this.meterId,
      })
      .subscribe(updated => {
        const index = this.uploadedDocuments.findIndex(d => d.id === doc.id);
        if (index > -1) this.uploadedDocuments[index] = updated;

        this.toaster.success('::SuccessfullyUpdated');
        this.editingInlineDocId = null;
      });
  }

  nextStep() {
    if (this.form.invalid) return;
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
}
