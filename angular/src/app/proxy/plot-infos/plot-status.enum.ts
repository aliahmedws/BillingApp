import { mapEnumToOptions } from '@abp/ng.core';

export enum PlotStatus {
  Available = 1,
  Reserved = 2,
  Sold = 3,
  UnderProcess = 4,
  Cancelled = 5,
  UnderConstruction = 6,
  Completed = 7,
  Leased = 8,
  Mortgaged = 9,
  Disputed = 10,
  Transferred = 11,
  Possessed = 12,
  Repossessed = 13,
  Inactive = 14,
  UnderReview = 15,
}

export const plotStatusOptions = mapEnumToOptions(PlotStatus);
