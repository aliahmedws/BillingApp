import { PagedResultDto, ListService } from '@abp/ng.core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BlockDto, GetBlockListDto, BlockService, UpdateBlockDto, CreateBlockDto } from '../proxy/blocks';
import { PhaseDto, PhaseService, GetPhaseListDto, PhaseLookUp } from '../proxy/phases';

@Component({
  selector: 'app-block',
  standalone: false,
  templateUrl: './block.component.html',
  styleUrl: './block.component.scss',
  providers: [ListService],
})
export class BlockComponent implements OnInit {
 blocks = { items: [], totalCount: 0 } as PagedResultDto<BlockDto>;
  form: FormGroup;
  isModalOpen = false;
  showFilter = false;
  selectedBlock = {} as BlockDto;

  filters = {} as GetBlockListDto;

  phaseLookup: PhaseLookUp[] = [];

  constructor(
    public readonly list: ListService,
    private blockService: BlockService,
    private phaseService: PhaseService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private toaster: ToasterService
  ) {}

  ngOnInit(): void {
    const streamCreator = (query) => this.blockService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.blocks = res));
    this.getPhaseLookup();
  }

  getPhaseLookup() {
    this.phaseService.getPhaseLookUp().subscribe((phases) => {
      this.phaseLookup = phases;
    });
  }

  private buildForm() {
    this.form = this.fb.group({
      blockCode: [this.selectedBlock.blockCode || '', Validators.required],
      blockName: [this.selectedBlock.blockName || '', Validators.required],
      phaseId: [this.selectedBlock.phaseId || null, Validators.required],
      description: [this.selectedBlock.description || null],
      isActive: [this.selectedBlock.isActive ?? true],
    });
  }

  createBlock() {
    this.selectedBlock = {} as BlockDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editBlock(id: string) {
    this.blockService.get(id).subscribe((b) => {
      this.selectedBlock = b;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) return;

    const dto = this.form.getRawValue();

    if (this.selectedBlock.id) {
      this.blockService.update(this.selectedBlock.id, dto as UpdateBlockDto).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::SuccessfullyUpdated');
      });
    } else {
      this.blockService.create(dto as CreateBlockDto).subscribe(() => {
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
        this.blockService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  clearFilters() {
    this.filters = {} as GetBlockListDto;
    this.list.get();
  }
}
