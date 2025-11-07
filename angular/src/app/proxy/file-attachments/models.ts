import type { EntityDto } from '@abp/ng.core';

export interface FileAttachmentDto extends EntityDto {
  name?: string;
  blobName?: string;
  path?: string;
  sizeInBytes: number;
  fileBytes: number[];
}
