import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { ConfirmationService, ToasterService, Confirmation } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { ConsumerPersonalInfoDto, GetConsumerPersonalInfoListDto, ConsumerPersonalInfoService } from '../proxy/consumer-personal-infos';

@Component({
  selector: 'app-consumer-personal-info',
  standalone: false,
  templateUrl: './consumer-personal-info.component.html',
  styleUrl: './consumer-personal-info.component.scss',
  providers: [ListService],
})
export class ConsumerPersonalInfoComponent implements OnInit {
  consumers = { items: [], totalCount: 0 } as PagedResultDto<ConsumerPersonalInfoDto>;
  filters = {} as GetConsumerPersonalInfoListDto;
  showFilter = false;

  constructor(
    public readonly list: ListService,
    private consumerService: ConsumerPersonalInfoService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
    private router: Router

  ) {}

  ngOnInit(): void {
    const streamCreator = (query) =>
      this.consumerService.getList({ ...query, ...this.filters });
    this.list.hookToQuery(streamCreator).subscribe((res) => (this.consumers = res));
  }

  editConsumer(id: string) {
    this.router.navigate(['/consumerPersonalInfoCreate'], { queryParams: { id, edit: true } });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.consumerService.delete(id).subscribe(() => {
          this.list.get();
          this.toaster.success('::SuccessfullyDeleted');
        });
      }
    });
  }

  viewConsumer(id: string) {
    this.router.navigate(['/consumerPersonalInfoCreate'], { queryParams: { id , view: true} });
  }

  clearFilters() {
    this.filters = {} as GetConsumerPersonalInfoListDto;
    this.list.get();
  }

  navigateToCreate() {
    this.router.navigate(['/consumerPersonalInfoCreate']);
  }
}
