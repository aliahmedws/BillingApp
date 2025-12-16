import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { GetSocietyChargeListDto, SocietyChargeDto, SocietyChargeService } from '../proxy/society-charges';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { PlotSizeLookupDto, PlotSizeService } from '../proxy/plot-sizes';

@Component({
  selector: 'app-societycharge',
  standalone: false,
  templateUrl: './societycharge.component.html',
  styleUrl: './societycharge.component.scss',
  providers: [ListService],
})

export class SocietychargeComponent implements OnInit {
societyCharges = { items: [], totalCount: 0 } as PagedResultDto<SocietyChargeDto>;
form: FormGroup;
isModalOpen = false;
isViewModalOpen = false;
showFilter = false;
selectedSocietyCharge = {} as SocietyChargeDto;
filters = {} as GetSocietyChargeListDto;
plotSizeLookup: PlotSizeLookupDto[] = [];

constructor(
  public readonly list: ListService,
  private societyChargeService: SocietyChargeService,
  private fb: FormBuilder,
  private confirmation: ConfirmationService,
  private toaster: ToasterService,
  private plotSizeService: PlotSizeService
) 
{}

ngOnInit(): void {
  const streamCreator = (query) => this.societyChargeService.getList({ ...query, ...this.filters });
  this.list.hookToQuery(streamCreator).subscribe((res) => (this.societyCharges = res));
  this.getPlotSizeLookup();
}

getPlotSizeLookup(){
  this.plotSizeService.getPlotSizeLookup().subscribe((plotsizes) => {
    this.plotSizeLookup = plotsizes;
  });
}

buildForm() {
  this.form = this.fb.group({
    plotSizeId: [this.selectedSocietyCharge.plotSizeId ?? null],
    securityCharges: [this.selectedSocietyCharge.securityCharges ?? 0],
    maintenanceCharges: [this.selectedSocietyCharge.maintenanceCharges ?? 0],
    waterCharges: [this.selectedSocietyCharge.waterCharges ?? 0],
    otherCharges: [this.selectedSocietyCharge.otherCharges ?? 0],
  });
}

createSocietyCharge() {
  this.selectedSocietyCharge = {} as SocietyChargeDto;
  this.buildForm();
    this.getPlotSizeLookup(); 
  this.isModalOpen = true;
}

editSocietyCharge(id: string) {
  this.societyChargeService.get(id).subscribe((societyCharge) => {
    this.selectedSocietyCharge = societyCharge;
    this.buildForm();
      this.getPlotSizeLookup(); 
    this.isModalOpen = true;
  });
}

save() {
  if (this.form.invalid) return;

   if (this.selectedSocietyCharge.id && !this.form.dirty) {
     this.toaster.info('::NoChangesDetected');
     return; 
    }

  const dto = this.form.getRawValue();

  if (this.selectedSocietyCharge.id) {
    this.societyChargeService.update(this.selectedSocietyCharge.id, dto as SocietyChargeDto).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
      this.toaster.success('::SuccessfullyUpdated');
    });
  } else {
    debugger;
    this.societyChargeService.create(dto as SocietyChargeDto).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
      this.toaster.success('::SuccessfullyCreated');
    });
  }
}

delete(id: string) {
  this.confirmation.warn('::AreYouSureToDelete', '::Confirmation').subscribe((status) => {
    if (status === Confirmation.Status.confirm) {
      this.societyChargeService.delete(id).subscribe(() => {
        this.list.get();
        this.toaster.success('::SuccessfullyDeleted');
      });
    }
  });
}


viewSocietyCharge(id: string) {
  this.societyChargeService.get(id).subscribe((societyCharge) => {
    this.selectedSocietyCharge = societyCharge;
    this.isViewModalOpen = true;
  });
}

}
