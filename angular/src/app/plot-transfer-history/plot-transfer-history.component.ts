import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConsumerPersonalInfoLookupDto, ConsumerPersonalInfoService } from '../proxy/consumer-personal-infos';
import { PlotInfoLookupDto, PlotInfoService } from '../proxy/plot-infos';
import { PlotTransferHistoryDto, GetPlotTransferHistoryListDto, transferTypeOptions, PlotTransferHistoryService } from '../proxy/plot-transfer-histories';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-plot-transfer-history',
  standalone: false,
  templateUrl: './plot-transfer-history.component.html',
  styleUrl: './plot-transfer-history.component.scss',
  providers: [ListService]
})
export class PlotTransferHistoryComponent implements OnInit{
transfers = { items: [], totalCount: 0 } as PagedResultDto<PlotTransferHistoryDto>;
  filters = {} as GetPlotTransferHistoryListDto;
  showFilter = false;
  approveModalVisible = false;
  rejectModalVisible = false;
  isApproveBusy = false;
  isRejectBusy = false;
  rejectionReasonModalVisible = false;
  selectedRejectionReason = '';
  selectedTransferId: string | null = null;
  transferTypes = transferTypeOptions;
  transferStatus = transferTypeOptions;
  plots = [] as PlotInfoLookupDto[];
  consumers = [] as ConsumerPersonalInfoLookupDto[];
  rejectionReason: string;
  rejectId: string;

  approveForm: FormGroup;
  rejectForm: FormGroup;

  constructor(
    public readonly list: ListService,
    private transferService: PlotTransferHistoryService,
    private plotService: PlotInfoService,
    private consumerService: ConsumerPersonalInfoService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    const streamCreator = (query) => this.transferService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.transfers = res));
    this.getPlots();
    this.getConsumers();
    this.buildForm();
  }

  buildForm() {
     this.rejectForm = this.fb.group({
      reason: ['', Validators.required]
    });
  }

  approve(id: string, currentStatus: number) {

    if(currentStatus === 2) {
      this.toaster.warn('::AlreadyApproved')
      return;
    }

    this.confirmation
    .info('::AreYouSureToApprove', '::Confirmation')
    .subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.submitApprove(id);
      }
    });
  }

  submitApprove(id: string) {
    this.transferService.approve(id).subscribe(() => {
      this.toaster.success('::ApprovedSuccessfully');
      this.isApproveBusy = false;
      this.list.get();
    })
  }

  reject(id: string, currentStatus: number, rejectionReason: string) {

    if (currentStatus === 3) {
    this.showRejectionReason(rejectionReason);
    return;
  }

  this.selectedTransferId = id;
  this.rejectForm.reset();
  this.rejectModalVisible = true;
}

showRejectionReason(reason: string) {
    this.selectedRejectionReason = reason;
    this.rejectionReasonModalVisible = true;
  }

save() {
  debugger;
  if (this.rejectForm.invalid || !this.selectedTransferId) return;

  this.isRejectBusy = true;
  const reason = this.rejectForm.value.reason;

  this.transferService.reject(this.selectedTransferId, reason).subscribe(() => {
      this.toaster.success('::RejectSuccessfully');
      this.rejectModalVisible = false;
      this.rejectForm.reset();
      this.selectedTransferId = null;
      this.list.get();
      this.isRejectBusy = false;
  });
}


  getPlots() {
    this.plotService.getPlotLookUp().subscribe((res) => (this.plots = res));
  }

  getConsumers() {
    this.consumerService.consumerPersonalInfoLookup().subscribe((res) => (this.consumers = res));
  }

  editTransfer(id: string) {
    this.router.navigate(['/CreatePlotTransferHistories'], { queryParams: { id, edit: true } });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.transferService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  viewTransfer(id: string) {
    this.router.navigate(['/CreatePlotTransferHistories'], { queryParams: { id, view: true } });
  }

  clearFilters() {
    this.filters = {} as GetPlotTransferHistoryListDto;
    this.list.get();
  }

  navigateToCreate() {
    this.router.navigate(['/CreatePlotTransferHistories']);
  }

  
}
