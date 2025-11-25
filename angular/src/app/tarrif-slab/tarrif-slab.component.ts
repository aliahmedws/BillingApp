import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { TarrifSlabService, TarrifSlabDto, GetTarrifSlabLIstDto, CreateTarrifSlabDto } from '../proxy/tarrif-slabs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation, ToasterService } from '@abp/ng.theme.shared';
import { query } from '@angular/animations';


@Component({
  selector: 'app-tarrif-slab',
  standalone: false,
  templateUrl: './tarrif-slab.component.html',
  styleUrls: ['./tarrif-slab.component.scss'],
  providers: [ListService],
})

export class TarrifSlabComponent implements OnInit {
  tarrifSlab = { items: [], totalCount: 0 } as PagedResultDto<TarrifSlabDto>;
  isModalOpen = false;
  showFilter = false;
  form: FormGroup;
  selectedTarrifSlab = {} as TarrifSlabDto;
  filters = {} as GetTarrifSlabLIstDto;

  constructor(
    public readonly list: ListService,
    private tarrifSlabService: TarrifSlabService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private toaster: ToasterService
  ) { }


  ngOnInit(): void {
    const tarrifSlabStreamCreator = (query) => this.tarrifSlabService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(tarrifSlabStreamCreator).subscribe((response) => { this.tarrifSlab = response; });
  }

  buildForm() {
    this.form = this.fb.group({
      lowerSlab: [this.selectedTarrifSlab.lowerSlab || '', Validators.required],
      upperSlab: [this.selectedTarrifSlab.upperSlab || ''],
      unitPrice: [this.selectedTarrifSlab.unitPrice || '', Validators.required],
    });
  }

  createTarrifSlab() {
    this.selectedTarrifSlab = {} as TarrifSlabDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editTarrifSlab(id: string) {
    this.tarrifSlabService.get(id).subscribe((tarrifSlab) => {
      this.selectedTarrifSlab = tarrifSlab;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
    debugger;
    if (this.form.invalid) return;

    if (this.selectedTarrifSlab.id) {
      this.tarrifSlabService.update(this.selectedTarrifSlab.id, this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::TarrifSlabUpdatedSuccessfully');
      });
    } else {
      this.tarrifSlabService.create(this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
        this.toaster.success('::TarrifSlabCreatedSuccessfully');
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.tarrifSlabService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::TarrifSlabDeletedSuccessfully');
        });
      }
    });
  }

  clearFilters() {
    this.filters = {} as GetTarrifSlabLIstDto;
    this.list.get();
    this.form.reset();
  }

}
