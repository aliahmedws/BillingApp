import type { Country } from './country.enum';
import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { Gender } from './gender.enum';

export interface AddressDto {
  street: string;
  city: string;
  state: string;
  country: Country;
  postalCode: string;
}

export interface ConsumerPersonalInfoDto extends EntityDto<string> {
  firstName?: string;
  lastName?: string;
  phone?: string;
  cnic?: string;
  gender?: Gender;
  dob?: string;
  email?: string;
  alternativePersonName?: string;
  alternativePersonPhone?: string;
  alternativePersonEmail?: string;
  alternativePersonCNIC?: string;
  address: AddressDto;
}

export interface ConsumerPersonalInfoLookupDto {
  id?: string;
  firstName?: string;
  lastName?: string;
}

export interface CreateConsumerPersonalInfoDto {
  firstName: string;
  lastName: string;
  phone: string;
  cnic: string;
  gender: Gender;
  dob: string;
  email?: string;
  alternativePersonName?: string;
  alternativePersonPhone?: string;
  alternativePersonEmail?: string;
  alternativePersonCNIC?: string;
  address: AddressDto;
}

export interface GetConsumerPersonalInfoListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  firstName?: string;
  lastName?: string;
  cnic?: string;
  gender?: Gender;
}

export interface UpdateConsumerPersonalInfoDto {
  firstName: string;
  lastName: string;
  phone: string;
  cnic: string;
  gender: Gender;
  dob: string;
  email?: string;
  alternativePersonName?: string;
  alternativePersonPhone?: string;
  alternativePersonEmail?: string;
  alternativePersonCNIC?: string;
  address: AddressDto;
}
