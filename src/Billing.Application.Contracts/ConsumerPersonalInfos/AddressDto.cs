using System.ComponentModel.DataAnnotations;

namespace Billing.ConsumerPersonalInfos;

public class AddressDto
{
    [Required]
    [StringLength(AddressConsts.MaxStreetLength)]
    public string Street { get; set; } = string.Empty;

    [Required]
    [StringLength(AddressConsts.MaxCityLength)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(AddressConsts.MaxStateLength)]
    public string State { get; set; } = string.Empty;

    [Required]
    public Country Country { get; set; }

    [Required]
    [StringLength(AddressConsts.MaxPostalCodeLength)]
    public string PostalCode { get; set; } = string.Empty;
}
