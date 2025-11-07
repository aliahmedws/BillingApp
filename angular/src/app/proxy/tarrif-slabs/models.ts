import type { EntityDto } from '@abp/ng.core';

export interface TarrifSlabDto extends EntityDto<string> {
  rateRangeOne: number;
  rateRangeTwo: number;
  rateRangeThree: number;
  rateRangeFour: number;
  rateRangeFive?: number;
  rateRangeSix?: number;
  rateRangeSeven?: number;
  rateRangeEight?: number;
}

export interface UpdateTarrifSlabDto {
  rateRangeOne: number;
  rateRangeTwo: number;
  rateRangeThree: number;
  rateRangeFour: number;
  rateRangeFive?: number;
  rateRangeSix?: number;
  rateRangeSeven?: number;
  rateRangeEight?: number;
}
