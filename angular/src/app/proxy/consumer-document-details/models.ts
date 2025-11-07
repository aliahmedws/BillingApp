import type { EntityDto } from '@abp/ng.core';
import type { DocumentType } from './document-type.enum';
import type { FileAttachmentDto } from '../file-attachments/models';

export interface ConsumerDocumentDetailDto extends EntityDto<string> {
  consumerDocumentId?: string;
  documentType?: DocumentType;
  issueDate?: string;
  expireDate?: string;
  description?: string;
  isVerified: boolean;
  verifiedDate?: string;
  verifiedBy?: string;
  consumerDocumentFile: FileAttachmentDto;
}

export interface CreateConsumerDocumentDetailDto {
  documentType: DocumentType;
  issueDate?: string;
  expireDate?: string;
  description?: string;
  isVerified: boolean;
  verifiedDate?: string;
  verifiedBy?: string;
  consumerDocumentFile: FileAttachmentDto;
}
