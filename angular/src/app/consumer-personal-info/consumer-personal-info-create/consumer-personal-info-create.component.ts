import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Country, Gender, ConsumerPersonalInfoService, CreateConsumerPersonalInfoDto, ConsumerPersonalInfoDto, countryOptions, genderOptions, UpdateConsumerPersonalInfoDto } from 'src/app/proxy/consumer-personal-infos';

@Component({
  selector: 'app-consumer-personal-info-create',
  standalone: false,
  templateUrl: './consumer-personal-info-create.component.html',
  styleUrl: './consumer-personal-info-create.component.scss'
})
export class ConsumerPersonalInfoCreateComponent implements OnInit {
 form: FormGroup;
  countries = countryOptions;
  genders = genderOptions;
  selectedConsumer = {} as CreateConsumerPersonalInfoDto;
  consumer = {} as ConsumerPersonalInfoDto;
  isViewMode = false;
  isEditMode = false;
  id: string | null = null;

  constructor(
    private fb: FormBuilder,
    private consumerService: ConsumerPersonalInfoService,
    private toaster: ToasterService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.queryParamMap.get('id');
    const view = this.route.snapshot.queryParamMap.get('view') === 'true';
    const edit = this.route.snapshot.queryParamMap.get('edit') === 'true';
    this.isViewMode = view;
    this.isEditMode = edit;
    
    this.buildForm();

    if(this.id) {
      this.consumerService.get(this.id).subscribe(data => {
        this.consumer = data;

        const formatDob = data.dob ? data.dob.split('T')[0] : null;

        this.form.patchValue({...data, dob: formatDob});
        if (this.isViewMode) {
          this.form.disable();
        }
      });
    }

  }

  buildForm() {
      const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // simple, reliable email regex
      const phonePattern = /^[0-9]{10,15}$/; // 10–15 digits (for international or local)
      const cnicPattern = /^[0-9]{13}$/;
    this.form = this.fb.group({
      firstName: [ this.selectedConsumer.firstName || '', [Validators.required, Validators.maxLength(100)]],
      lastName: [ this.selectedConsumer.lastName || '', [Validators.required, Validators.maxLength(100)]],
      phone: [this.selectedConsumer.phone || '', [Validators.required, Validators.pattern(phonePattern)]],
      cnic: [this.selectedConsumer.cnic || '', [Validators.required, Validators.pattern(cnicPattern)]],
      gender: [this.selectedConsumer.gender || null, Validators.required],
      dob: [this.selectedConsumer.dob || null, Validators.required],
      email: [this.selectedConsumer.email || null, [Validators.required, Validators.pattern(emailPattern)]],
      alternativePersonName: [ this.selectedConsumer.alternativePersonName || '', [Validators.maxLength(100)]],
      alternativePersonPhone: [this.selectedConsumer.alternativePersonPhone || '', [Validators.pattern(phonePattern), Validators.maxLength(15)]],
      alternativePersonEmail: [this.selectedConsumer.alternativePersonEmail || '', [Validators.pattern(emailPattern), Validators.maxLength(150)]],
      alternativePersonCNIC: [this.selectedConsumer.alternativePersonCNIC || '', [Validators.pattern(cnicPattern), Validators.maxLength(13)]],
      address: this.fb.group({
        street: [this.selectedConsumer.address?.street || '', [Validators.required, Validators.maxLength(200)]],
        city: [this.selectedConsumer.address?.city || '', [Validators.required, Validators.maxLength(100)]],
        state: [this.selectedConsumer.address?.state || '', [Validators.required, Validators.maxLength(100)]],
        country: [this.selectedConsumer.address?.country || null, Validators.required],
        postalCode: [this.selectedConsumer.address?.postalCode || '', [Validators.required, Validators.maxLength(20)]],
      }),
    });
  }

  save() {
    if (this.isViewMode) {
      this.router.navigate(['/consumerPersonalInfos']);
      return;
    }
    if (this.form.invalid) return;

    const dto = this.form.value as CreateConsumerPersonalInfoDto | UpdateConsumerPersonalInfoDto;

    if(this.isEditMode && this.id) {
      this.consumerService.update(this.id, dto).subscribe(() => {
        this.toaster.success('::SuccessfullyUpdated');
       this.backToList();
      });
    } else {
    this.consumerService.create(dto).subscribe(() => {
      this.toaster.success('::SuccessfullyCreated');
      this.backToList();
    });
  }}

  backToList() {
    this.router.navigate(['/consumerPersonalInfos']);
  }
}
