import { ToasterService, ConfirmationService, Confirmation } from "@abp/ng.theme.shared";
import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { BlockLookupDto, BlockService } from "src/app/proxy/blocks";
import { ConsumerPersonalInfoLookupDto, ConsumerPersonalInfoService } from "src/app/proxy/consumer-personal-infos";
import { PhaseLookUp, PhaseService } from "src/app/proxy/phases";
import { plotDocumentTypeOptions, PlotDocumentDto, PlotDocumentService } from "src/app/proxy/plot-documents";
import { PlotInfoDto, plotStatusOptions, PlotInfoService, PlotStatus, CreatePlotInfoDto, UpdatePlotInfoDto } from "src/app/proxy/plot-infos";
import { PlotSizeLookupDto, PlotSizeService } from "src/app/proxy/plot-sizes";
import { plotTypeOptions } from "src/app/proxy/plot-types";
import { CustomPlotDocumentService } from "src/custom-services/upload-document-services/plot-document-service";


@Component({
  selector: 'app-create-plot-info',
  standalone: false,
  templateUrl: './create-plot-info.component.html',
  styleUrl: './create-plot-info.component.scss',
})
export class CreatePlotInfoComponent implements OnInit {
  form: FormGroup;
  isViewMode = false;
  // isEditMode = false;
  // isDragOver = false;
  id: string | null = null;
  plotId: string = null;

  plot = {} as PlotInfoDto;
  selectedPlotInformation = {} as PlotInfoDto;

  plotTypes = plotTypeOptions;
  plotStatus = plotStatusOptions;
  plotSizes: PlotSizeLookupDto[] = [];
  blocks: BlockLookupDto[] = [];
  phases: PhaseLookUp[] = [];
  consumers: ConsumerPersonalInfoLookupDto[] = [];

  // documents
  plotDocumentType = plotDocumentTypeOptions;
  uploadedDocuments: PlotDocumentDto[] = [];
  editingInlineDocId: string | null = null;
  selectedFile: File | null = null;
  selectedDocumentType: number | null = null;
  description = '';
  documentNo = '';
  issueDate: string | null = null;
  expireDate: string | null = null;

  currentStep = 0;

  constructor(
    private fb: FormBuilder,
    private plotService: PlotInfoService,
    private plotSizeService: PlotSizeService,
    private phaseService: PhaseService,
    private blockService: BlockService,
    private consumerService: ConsumerPersonalInfoService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute,
    private customPlotlotDocumentService: CustomPlotDocumentService,
    private plotDocumentService: PlotDocumentService,
    private confirmation: ConfirmationService
  )
  {}

  ngOnInit(): void {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.plotId = this.id;
    this.isViewMode = this.route.snapshot.queryParamMap.get('view') === 'true';

    this.buildForm();

    if (this.id) {
      this.plotService.get(this.id).subscribe(data => {
        this.plot = data;
        this.form.patchValue(data);

        // Load plot documents if they come with PlotInfoDto
        this.uploadedDocuments = (data as any).plotDocuments || [];

        if (data.phaseId) {
          this.blockService.getBlocksByPhaseId(data.phaseId).subscribe(res => {
            this.blocks = res || [];

            if(data.blockId){
              this.form.patchValue({ blockId: data.blockId });
            }
          });
        }

        if (this.isViewMode) {
          this.form.disable();
        }
      });
    }

    this.getPlotSizes();
    this.getBlocks();
    this.getPhases();
    this.getConsumer();
  }

  getConsumer() {
    this.consumerService.consumerPersonalInfoLookup().subscribe(res => {
      this.consumers = res;
    });
  }

  getPlotSizes() {
    this.plotSizeService.getPlotSizeLookup().subscribe(res => {
      this.plotSizes = res;
    });
  }

  getBlocks() {
    this.blockService.getBlockLookup().subscribe(res => {
      this.blocks = res;
    });
  }

  getPhases() {
    this.phaseService.getPhaseLookUp().subscribe(res => {
      this.phases = res;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      plotNo: [
        this.selectedPlotInformation.plotNo || '',
        [Validators.required, Validators.maxLength(64)],
      ],
      streetNo: [
        this.selectedPlotInformation.streetNo || '',
        [Validators.required, Validators.maxLength(64)],
      ],
      plotType: [this.selectedPlotInformation.plotType || null, Validators.required],
      plotSizeId: [this.selectedPlotInformation.plotSizeId || null, Validators.required],
      blockId: [this.selectedPlotInformation.blockId || null, Validators.required],
      phaseId: [this.selectedPlotInformation.phaseId || null, Validators.required],
      consumerId: [this.selectedPlotInformation.consumerId || null, Validators.required],
      status: [this.selectedPlotInformation.status || PlotStatus.Available, Validators.required],
      remarks: [this.selectedPlotInformation.remarks || '', Validators.maxLength(512)],
    });
  }

  saveAndNext() {
    if (this.form.invalid) {
      return;
    }

    const dto = this.form.value as CreatePlotInfoDto | UpdatePlotInfoDto;

    if (this.id) {
      this.plotService.update(this.id, dto).subscribe(() => {
        this.nextStep();
      });
    } else {
      this.plotService.create(dto).subscribe(res => {
        this.id = res.id;
        this.plotId = res.id;
        this.nextStep();
      });
    }
  }

  backToList() {
    this.router.navigate(['/plotInfos']);
  }

  onPhaseChange(phaseId: string) {
    this.form.patchValue({ blockId: null });
    this.blocks = [];

    if (!phaseId) return;

    this.blockService.getBlocksByPhaseId(phaseId).subscribe(res => {
      this.blocks = res ?? [];
    });
  }

  get selectedPhaseId(): string | null {
    return this.form.get('phaseId')?.value;
  }

  // ---------- Document Upload Logic (like Meter) ----------

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

    const Document_Type_Other = 13;
    if(this.selectedDocumentType === Document_Type_Other && !this.description){
      this.toaster.warn('::PleaseEnterDescriptionForOtherDocumentType');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('input.plotInfoId', this.plotId);
    formData.append('input.plotDocumentType', this.selectedDocumentType.toString());

    if (this.description) {
      formData.append('input.description', this.description);
    }
    if (this.documentNo) {
      formData.append('input.documentNumber', this.documentNo);
    }
    if (this.issueDate) {
      formData.append('input.issueDate', this.issueDate);
    }
    if (this.expireDate) {
      formData.append('input.expireDate', this.expireDate);
    }

    // If you have a custom OwnPlotDocumentService, use that instead of plotDocumentService.
    this.customPlotlotDocumentService.uploadFormData(formData).subscribe({
      next: (res: any) => {
        this.toaster.success('::Fileuploadedsuccessfully');
        this.uploadedDocuments.push(res);
        this.selectedFile = null;
        this.description = '';
        this.documentNo = '';
        this.issueDate = null;
        this.expireDate = null;
        this.selectedDocumentType = null;
      },
      error: err => {
        this.toaster.error('::Uploadfailed');
        console.error(err);
      },
    });
  }

  uploadAndFinish() {
    debugger;
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
    formData.append('input.plotInfoId', this.plotId!);
    formData.append('input.type', this.selectedDocumentType.toString());

    if (this.description) {
      formData.append('input.description', this.description);
    }
    if (this.documentNo) {
      formData.append('input.documentNumber', this.documentNo);
    }
    if (this.issueDate) {
      formData.append('input.issueDate', this.issueDate);
    }
    if (this.expireDate) {
      formData.append('input.expireDate', this.expireDate);
    }

    this.customPlotlotDocumentService.uploadFormData(formData as any).subscribe({
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

  deleteDocument(id: string, showConfirm: boolean = true) {
    const doDelete = () => {
      this.plotDocumentService.delete(id).subscribe(() => {
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

  saveInlineEdit(doc: PlotDocumentDto) {
    debugger;
    if (!doc.plotDocumentType) {
      this.toaster.warn('::PleaseCompleteTheFields');
      return;
    }

    const Document_Type_Other = 13;
    if(doc.plotDocumentType === Document_Type_Other && !doc.description){
      this.toaster.warn('::PleaseEnterDescriptionForOtherDocumentType');
      return;
    }

    this.plotDocumentService
      .update(doc.id, {
        plotDocumentType: doc.plotDocumentType,
        description: doc.description,
        plotInfoId: this.plotId,
        documentNumber: doc.documentNumber,
        issueDate: doc.issueDate,
        expireDate: doc.expireDate,
        isVerified: doc.isVerified ?? false,
      })
      .subscribe(updated => {
        const index = this.uploadedDocuments.findIndex(d => d.id === doc.id);
        if (index > -1) this.uploadedDocuments[index] = updated as any;

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
    this.form.enable();
  }
}