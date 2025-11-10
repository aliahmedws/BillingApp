import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ConsumerDocumentDetailDto, CreateConsumerDocumentDetailDto } from '../consumer-document-details/models';
import type { DocumentType } from '../consumer-document-details/document-type.enum';

export interface ConsumerDocumentDto extends EntityDto<string> {
  consumerId?: string;
  consumerDocumentDetails: ConsumerDocumentDetailDto[];
}

export interface CreateConsumerDocumentDto {
  consumerId: string;
  documentDetails: CreateConsumerDocumentDetailDto[];
}

export interface GetConsumerDocumentListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  consumerId?: string;
  documentType?: DocumentType;
  isVerified?: boolean;
  issueDate?: string;
  expireDate?: string;
}
