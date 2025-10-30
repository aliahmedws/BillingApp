using AutoMapper;
using Billing.GovtCharges;
using Billing.IescoCharges;
using Billing.Phases;
using Billing.SocietyCharges;

namespace Billing;

public class BillingApplicationAutoMapperProfile : Profile
{
    public BillingApplicationAutoMapperProfile()
    {
        CreateMap<Phase, PhaseDto>();
        CreateMap<GovtCharge, GovtChargeDto>();
        CreateMap<IescoCharge, IescoChargeDto>();
        CreateMap<SocietyCharge, SocietyChargeDto>();
    }
}
