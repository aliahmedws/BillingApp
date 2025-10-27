import { PagedResultDto, ListService } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PhaseDto, GetPhaseListDto, PhaseService, UpdatePhaseDto, CreatePhaseDto } from '../proxy/phases';

@Component({
  selector: 'app-phase',
  standalone: false,
  templateUrl: './phase.component.html',
  styleUrl: './phase.component.scss',
  providers: [ListService],
})
export class PhaseComponent implements OnInit {
phases = { items: [], totalCount: 0 } as PagedResultDto<PhaseDto>;
  form: FormGroup;
  isModalOpen = false;
  isViewModalOpen = false;
  showFilter = false;
  selectedPhase = {} as PhaseDto;
  filters = {} as GetPhaseListDto;

  constructor(
    public readonly list: ListService,
    private phaseService: PhaseService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private toaster: ToasterService
  ) {}

  ngOnInit(): void {
    const streamCreator = (query) => this.phaseService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.phases = res));
  }

  private buildForm() {
    this.form = this.fb.group({
      phaseCode: [this.selectedPhase.phaseCode || null],
      phaseName: [this.selectedPhase.phaseName || '', Validators.required],
      description: [this.selectedPhase.description || null],
      isActive: [this.selectedPhase.isActive ?? true],
    });
  }

  createPhase() {
    this.selectedPhase = {} as PhaseDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editPhase(id: string) {
    this.phaseService.get(id).subscribe((phase) => {
      this.selectedPhase = phase;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) return;

    const dto = this.form.getRawValue();

    if (this.selectedPhase.id) {
      this.phaseService.update(this.selectedPhase.id, dto as UpdatePhaseDto).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::SuccessfullyUpdated');
      });
    } else {
      this.phaseService.create(dto as CreatePhaseDto).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::SuccessfullyCreated');
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.phaseService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  viewPhaseDetails(id: string) {
    this.phaseService.get(id).subscribe((phase) => {
      this.selectedPhase = phase;
      this.isViewModalOpen = true;
    });
  }

  clearFilters() {
    this.filters = {} as GetPhaseListDto;
    this.list.get();
  }
}
