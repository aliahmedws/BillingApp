using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace Billing.ConsumerPersonalInfos;

public class Address : ValueObject
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public Country Country { get; set; }
    public string PostalCode { get; set; }

    private Address() { }

    public Address(
        string street,
        string city,
        string state,
        Country country,
        string postalCode)
    {
        SetStreet(street);
        SetCity(city);
        SetState(state);
        Country = country;
        SetPostalCode(postalCode);
    }

    private void SetStreet(string street)
    {
        Street = Check.NotNullOrWhiteSpace(street, nameof(street), AddressConsts.MaxStreetLength);
    }

    private void SetCity(string city)
    {
        City = Check.NotNullOrWhiteSpace(city, nameof(city), AddressConsts.MaxCityLength);
    }

    private void SetState(string state)
    {
        State = Check.NotNullOrWhiteSpace(state, nameof(state), AddressConsts.MaxStateLength);
    }

    private void SetPostalCode(string postalCode)
    {
        PostalCode = Check.NotNullOrWhiteSpace(postalCode, nameof(postalCode), AddressConsts.MaxPostalCodeLength);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return PostalCode;
    }
}