import { ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import {
  ConsumerPersonalInfoLookupDto,
  ConsumerPersonalInfoService,
} from 'src/app/proxy/consumer-personal-infos';
import { plotHistoryDocumentTypeOptions } from 'src/app/proxy/plot-documents';
import { PlotInfoLookupDto, PlotInfoService } from 'src/app/proxy/plot-infos';
import {
  PlotTransferHistoryDto,
  transferTypeOptions,
  PlotTransferHistoryService,
  CreatePlotTransferHistoryDto,
  UpdatePlotTransferHistoryDto,
} from 'src/app/proxy/plot-transfer-histories';
import {
  PlotTransferHistoryDocumentDto,
  PlotTransferHistoryDocumentService,
} from 'src/app/proxy/plot-transfer-history-documents';
import { CustomPlotHistoryDocumentService } from 'src/app/upload-document-services/plot-history-document';

@Component({
  selector: 'app-create-plot-transfer-history',
  standalone: false,
  templateUrl: './create-plot-transfer-history.component.html',
  styleUrl: './create-plot-transfer-history.component.scss',
  providers: [ListService],
})
export class CreatePlotTransferHistoryComponent implements OnInit {
  form: FormGroup;
  isViewMode = false;
  isNew = true;
  id: string | null = null;
  plotId: string | null = null;
  approveModalVisible = false;
  rejectModalVisible = false;
  rejectionReasonModalVisible = false;

  isApproveBusy = false;
  isRejectBusy = false;

  selectedRejectionReason = '';
  rejectForm: FormGroup;

  transfer = {} as PlotTransferHistoryDto;
  transferTypes = transferTypeOptions;
  plots = [] as PlotInfoLookupDto[];
  consumers = [] as ConsumerPersonalInfoLookupDto[];

  selectedTransfer = {} as PlotTransferHistoryDto;

  currentStep = 0;

  // documents
  transferDocumentTypes = plotHistoryDocumentTypeOptions;
  uploadedDocuments: PlotTransferHistoryDocumentDto[] = [];
  editingInlineDocId: string | null = null;
  selectedFile: File | null = null;
  selectedDocumentType: number | null = null;
  description = '';
  issueDate: string | null = null;
  expireDate: string | null = null;

  constructor(
    private fb: FormBuilder,
    private transferService: PlotTransferHistoryService,
    private plotService: PlotInfoService,
    private consumerService: ConsumerPersonalInfoService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute,
    private plotTransferDocumentService: PlotTransferHistoryDocumentService,
    private customPlotTransferDocumentService: CustomPlotHistoryDocumentService,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.queryParamMap.get('id');
    this.plotId = this.route.snapshot.queryParamMap.get('plotId');
    this.isViewMode = this.route.snapshot.queryParamMap.get('view') === 'true';
    this.isNew = !this.id;
    this.buildForm();

    if (this.id) {
      this.transferService.get(this.id).subscribe(data => {
        this.transfer = data;

        this.uploadedDocuments = (data as any).plotTransferHistoryDocuments  || [];
        
        const formattedDate = data.transferDate ? data.transferDate.split('T')[0] : null;
        this.form.patchValue({ ...data, transferDate: formattedDate });
        if (this.isViewMode) this.form.disable();
      });
    } else if (this.plotId) {
      this.form.patchValue({ plotId: this.plotId }, { emitEvent: false });
      this.onPlotChange(this.plotId);
    }

    this.getPlots();
    this.getConsumers();
  }

  onPlotChange(plotId: string) {
    if(!this.isNew) {
      return;
    }

    if (!plotId) {
      this.form.get('fromConsumerId')?.reset();
      return;
    }

    this.plotService.getPlotOwner(plotId).subscribe(res => {
      if (res && res.consumerId) {
        this.form.get('fromConsumerId')?.setValue(res.consumerId);
        this.form.get('fromConsumerId')?.disable();

        this.form.get('plotId')?.disable();
      } else {
        this.form.get('fromConsumerId').reset();
        this.form.get('plotId').reset();
      }
    });
  }

  getPlots() {
    this.plotService.getPlotLookUp().subscribe(res => (this.plots = res));
  }

  getConsumers() {
    this.consumerService.consumerPersonalInfoLookup().subscribe(res => (this.consumers = res));
  }

  buildForm() {
    this.form = this.fb.group({
      plotId: [this.selectedTransfer.plotId || null, Validators.required],
      fromConsumerId: [this.selectedTransfer.fromConsumerId || null, Validators.required],
      toConsumerId: [this.selectedTransfer.toConsumerId || null, Validators.required],
      transferDate: [this.selectedTransfer.transferDate || null, Validators.required],
      transferType: [this.selectedTransfer.transferType || null, Validators.required],
      registryNo: [
        this.selectedTransfer.registryNo || '',
        [Validators.required, Validators.maxLength(100)],
      ],
      remarks: [this.selectedTransfer.remarks || '', Validators.maxLength(512)],
    });
  }

  buildRejectForm() {
    this.rejectForm = this.fb.group({
      reason: ['', Validators.required],
    });
  }

  save() {
    if (this.isViewMode) {
      this.backToList();
      return;
    }
    if (this.form.invalid) return;

    this.form.get('fromConsumerId')?.enable({ emitEvent: false });
    this.form.get('plotId')?.enable({ emitEvent: false });

    const dto = this.form.value as CreatePlotTransferHistoryDto | UpdatePlotTransferHistoryDto;

    if (this.id) {
      this.transferService.update(this.id, dto).subscribe(() => {
        this.nextStep();
      });
    } else {
      this.transferService.create(dto).subscribe(res => {
        this.toaster.success('::SuccessfullyCreated');
        this.id = res.id;
        this.nextStep();
      });
    }
  }

  onFileSelected(file: File | null) {
    this.selectedFile = file;
  }

  upload() {
    debugger;
    if (!this.selectedFile) {
      this.toaster.warn('::Pleaseselectafilefirst');
      return;
    }

    if (!this.id) {
      this.toaster.error('::TransferNotSavedYet');
      return;
    }

    if (!this.selectedDocumentType) {
      this.toaster.warn('::PleaseSelectDocumentType');
      return;
    }

    const Document_Type_Other = 8;
    if( this.selectedDocumentType === Document_Type_Other && !this.description){
      this.toaster.warn('::PleaseEnterDescriptionForOtherDocumentType');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('input.plotTransferHistoryId', this.id);
    formData.append('input.plotHistoryDT', this.selectedDocumentType.toString());

    if (this.description) {
      formData.append('input.remarks', this.description);
    }
    if (this.issueDate) {
      formData.append('input.issueDate', this.issueDate);
    }
    if (this.expireDate) {
      formData.append('input.expireDate', this.expireDate);
    }

    formData.append('input.isVerified', 'false');

    this.customPlotTransferDocumentService.uploadFormData(formData).subscribe({
      next: (res: any) => {
        this.toaster.success('::Fileuploadedsuccessfully');
        this.uploadedDocuments.push(res);
        this.selectedFile = null;
        this.description = '';
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
    if (!this.selectedFile) {
      this.toaster.success('::UpdateSuccessfully');
      this.backToList();
      return;
    }

    if (!this.id) {
      this.toaster.error('::TransferNotSavedYet');
      return;
    }

    if (!this.selectedDocumentType) {
      this.toaster.warn('::PleaseSelectDocumentType');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('input.plotTransferHistoryId', this.id);
    formData.append('input.plotHistoryDT', this.selectedDocumentType.toString());

    if (this.description) {
      formData.append('input.remarks', this.description);
    }
    if (this.issueDate) {
      formData.append('input.issueDate', this.issueDate);
    }
    if (this.expireDate) {
      formData.append('input.expireDate', this.expireDate);
    }

    formData.append('input.isVerified', 'false');

    this.customPlotTransferDocumentService.uploadFormData(formData).subscribe({
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
      this.plotTransferDocumentService.delete(id).subscribe(() => {
        this.uploadedDocuments = this.uploadedDocuments.filter(d => d.id !== id);
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

  saveInlineEdit(doc: PlotTransferHistoryDocumentDto) {
    if (!doc.plotHistoryDT) {
      this.toaster.warn('::PleaseCompleteTheFields');
      return;
    }

    const Document_Type_Other = 8;
    if( doc.plotHistoryDT === Document_Type_Other && !doc.remarks){
      this.toaster.warn('::PleaseEnterDescriptionForOtherDocumentType');
      return;
    }

    this.plotTransferDocumentService
      .update(doc.id, {
        plotTransferHistoryId: this.id,
        plotHistoryDT: doc.plotHistoryDT,
        remarks: doc.remarks,
        issueDate: doc.issueDate,
        expireDate: doc.expireDate,
        isVerified: doc.isVerified ?? false,
      })
      .subscribe(updated => {
        const index = this.uploadedDocuments.findIndex(d => d.id === doc.id);
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
    this.form.enable();
  }

  approve() {
    if (!this.id) return;

    this.confirmation.info('::AreYouSureToApprove', '::Confirmation').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.submitApprove();
      }
    });
  }

  submitApprove() {
    this.isApproveBusy = true;

    this.transferService.approve(this.id).subscribe(() => {
      this.isApproveBusy = false;
      this.toaster.success('::ApprovedSuccessfully');
    });
  }

  reject() {
    if (!this.id) return;

    if (this.transfer.rejectionReason != null) {
      this.showRejectionReason(this.transfer.rejectionReason || '');
      return;
    }

    this.rejectForm.reset();
    this.rejectModalVisible = true;
  }

  saveRejection() {
    if (this.rejectForm.invalid || !this.id) return;

    this.isRejectBusy = true;
    const reason = this.rejectForm.value.reason;

    this.transferService.reject(this.id, reason).subscribe(() => {
      this.toaster.success('::RejectSuccessfully');
      this.rejectModalVisible = false;
      this.isRejectBusy = false;
    });
  }

  showRejectionReason(reason: string) {
    this.selectedRejectionReason = reason;
    this.rejectionReasonModalVisible = true;
  }

  backToList() {
    this.router.navigate(['/plotTransferHistories']);
  }
}
