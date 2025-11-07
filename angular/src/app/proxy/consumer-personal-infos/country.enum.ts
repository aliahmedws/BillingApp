import { mapEnumToOptions } from '@abp/ng.core';

export enum Country {
  Pakistan = 1,
  UnitedStates = 2,
  Canada = 3,
  UnitedKingdom = 4,
  UnitedArabEmirates = 5,
  SaudiArabia = 6,
  India = 7,
  China = 8,
  Australia = 9,
  Germany = 10,
  Other = 11,
}

export const countryOptions = mapEnumToOptions(Country);
